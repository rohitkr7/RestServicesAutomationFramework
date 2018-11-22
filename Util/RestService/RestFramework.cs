using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace RestServicesAutomationFramework.Util.RestService
{
    class RestFramework
    {
        private IRestClient client = null;
        private IRestRequest request = null;
        private IRestResponse response = null;

        /// <summary>
        /// This method takes the TestCaseId and the Endpoint URL of the API and returns the json response as a string after doing GET Operation.
        /// </summary>
        /// <param name="TestCaseId"></param>
        /// <param name="endPointUrl"></param>
        /// <returns></returns>
        public string NormalGetOperation(string TestCaseId, string endPointUrl)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

            string jsonResponse = null;

            client = new RestClient()
            {
                BaseUrl = new Uri(endPointUrl),
            };

            request = new RestRequest();
            request.Method = Method.GET;

            response = new RestResponse();
            response = client.Execute(request);

            jsonResponse = response.Content.ToString();

            return jsonResponse;
        }

        /// <summary>
        /// This method is for getting 
        /// </summary>
        /// <param name="endPointUrl"></param>
        /// <param name="headerSet"></param>
        /// <param name="client_id"></param>
        /// <param name="client_secret"></param>
        /// <returns></returns>
        public string OAuth_2_GetAccessToken(string testCaseId, string endPointUrl, string headerSet, string client_id, string client_secret)
        {
            string jsonResponse = null;

            client = new RestClient()
            {
                BaseUrl = new Uri(endPointUrl),
            };

            request = new RestRequest();
            request.Method = Method.GET;

            response = new RestResponse();
            response = client.Execute(request);

            if (headerSet != null && headerSet != "")
            {
                string[] headers = SplitToArray(headerSet, ',');
                foreach (var header in headers)
                {
                    if (header.Contains(':'))
                    {
                        string key = header.Split(':')[0];
                        string value = header.Split(':')[1];

                        request.AddHeader(key, value);
                    }
                    else if (header.Contains('='))
                    {
                        string key = header.Split('=')[0];
                        string value = header.Split('=')[1];

                        request.AddHeader(key, value);
                    }

                }
            }

            if (client_id != null && client_id != "" && client_secret != null && client_secret != "")
            {
                request.AddHeader("Client_ID", client_id);
                request.AddHeader("Client_Secret", client_secret);
            }

            jsonResponse = response.Content.ToString();
            string accessToken = (string)JObject.Parse(jsonResponse).SelectToken("access_token");
            return accessToken;
        }

        private string[] SplitToArray(string sentence, char delimeter)
        {

            if (sentence != null && sentence != "" && delimeter != '\0')
            {
                if (sentence.Contains(delimeter))
                {
                    return sentence.Split(delimeter);
                }
                else
                {
                    return new String[] { sentence };
                }
            }
            else
            {
                Console.WriteLine("Some of the parameter may be blank or null.");
                return null;
            }
        }

        /// <summary>
        /// This method is for POST operation for an RESTful API.
        /// </summary>
        /// <param name="endPointUrl"></param>
        /// <param name="headerSet"></param>
        /// <param name="payload"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public string Normal_Post_Put_Operation(string testCaseId, string endPointUrl,Method httpMethod, string headerSet, string payload, string accessToken)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

            string jsonResponse = null;

            client = new RestClient()
            {
                BaseUrl = new Uri(endPointUrl),
            };

            request = new RestRequest();
            request.Method = httpMethod;
            request.AddHeader("cache-control", "no-cache");
            //request.AddHeader("content-type", "application/json");


            if (headerSet != null && headerSet != "")
            {
                string[] headers = SplitToArray(headerSet, ',');
                foreach (var header in headers)
                {
                    if (header.Contains(':'))
                    {
                        string key = header.Split(':')[0];
                        string value = header.Split(':')[1];

                        request.AddHeader(key, value);
                    }
                    else if (header.Contains('='))
                    {
                        string key = header.Split('=')[0];
                        string value = header.Split('=')[1];

                        request.AddHeader(key, value);
                    }

                }
            }

            if (payload != null && payload != "")
            {
                request.AddParameter("application/json", payload, ParameterType.RequestBody);
                //request.AddBody(payload);
            }

            if (accessToken != "" && accessToken != null)
            {
                request.AddHeader("Authorization", "Bearer " + accessToken);
            }

            response = new RestResponse();
            response = client.Execute(request);

            jsonResponse = response.Content.ToString();

            return jsonResponse;
        }

        /// <summary>
        /// This method returns a value corresponding to a particular json token path. 
        /// </summary>
        /// <param name="jsonResponse"></param>
        /// <param name="jsonPath"></param>
        /// <returns></returns>
        public string GetSpecificTokenValue(string jsonResponse, string jsonPath)
        {
            if (jsonPath.Contains('*'))
            {
                Console.WriteLine("The json path is generic. It needs to be specific for the element you want.");
                return null;
            }
            string value = (string)JObject.Parse(jsonResponse).SelectToken(jsonPath);
            return value;
        }

        /// <summary>
        /// This method returns a value corresponding to a particular json token path and handles the exception as well.
        /// </summary>
        /// <param name="jsonResponse"></param>
        /// <param name="jsonPath"></param>
        /// <returns></returns>
        public string TryToGetSpecificTokenValue(string jsonResponse, string jsonPath)
        {
            try
            {
                return GetSpecificTokenValue(jsonResponse, jsonPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("No Such JSON Token Exists: " + jsonPath);
                return "";
            }
        }

        /// <summary>
        /// This method is to get multiple values having the same jsonPath in the Response JSON.
        /// </summary>
        /// <param name="jsonResponse"></param>
        /// <param name="jsonPath"></param>
        /// <returns></returns>
        public List<string> RetrieveMultipleTokens(string jsonResponse, string jsonPath)
        {
            List<string> tokenValuesList = new List<string>();
            var tokenValues = JObject.Parse(jsonResponse).SelectTokens(jsonPath);
            foreach (var tokenValue in tokenValues)
            {
                tokenValuesList.Add(tokenValue.ToString());
                Console.WriteLine(tokenValue);
            }

            return tokenValuesList;
        }

        /// <summary>
        /// This is a generic method to execute any of the GET, POST, PUT or PATCH operations.
        /// </summary>
        /// <param name="testCaseId"></param>
        /// <param name="endPointUrl"></param>
        /// <param name="headerSet"></param>
        /// <param name="httpOperation"></param>
        /// <param name="requestPayload"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public string GenericHttpOperation_OAuth(string testCaseId, string endPointUrl, string headerSet,string parameters, Method httpMethod, string requestPayload, string accessToken)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            string jsonResponse = null;

            client = new RestClient()
            {
                BaseUrl = new Uri(endPointUrl),
            };

            request = new RestRequest(httpMethod);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");

            if (headerSet != null && headerSet != "")
            {
                string[] headers = SplitToArray(headerSet, ',');
                foreach (var header in headers)
                {
                    if (header.Contains(':'))
                    {
                        string key = header.Split(':')[0];
                        string value = header.Split(':')[1];

                        request.AddHeader(key, value);
                    }
                    else if (header.Contains('='))
                    {
                        string key = header.Split('=')[0];
                        string value = header.Split('=')[1];

                        request.AddHeader(key, value);
                    }
                }
            }

            if (parameters != null && parameters != "")
            {
                string[] parametersArray = SplitToArray(parameters, ',');
                foreach (string param in parametersArray)
                {
                    if (param.Contains(':'))
                    {
                        string key = param.Split(':')[0];
                        string value = param.Split(':')[1];

                        request.AddHeader(key, value);
                    }
                    else if (param.Contains('='))
                    {
                        string key = param.Split('=')[0];
                        string value = param.Split('=')[1];

                        request.AddQueryParameter(key, value);
                    }
                }
            }

            if (accessToken != "" && accessToken != null)
            {
                request.AddHeader("Authorization", "Bearer " + accessToken);
            }

            if (requestPayload != null && requestPayload != "")
            {
                request.AddParameter("application/json", requestPayload, ParameterType.RequestBody);
                //request.AddBody(payload);
            }

            response = new RestResponse();
            response = client.Execute(request);
            jsonResponse = response.Content.ToString();
            Console.WriteLine(jsonResponse);
            return jsonResponse;
        }

        /// <summary>
        /// This method is to get the Current HttpStatus Code of the REST response.
        /// </summary>
        /// <returns></returns>
        public HttpStatusCode getResponseStatus()
        {
            return response.StatusCode;
        }

        /// <summary>
        /// This method returns current instance of RestResponse Class
        /// </summary>
        /// <returns></returns>
        public IRestResponse getRestResponseInstance()
        {
            return response;
        }

        /// <summary>
        /// This method takes jsonResponse of array type and a jsonArrayName which is the token name, and returns a list of all the corresponding values.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="jsonArrayName"></param>
        /// <returns></returns>
        public List<string> GetJsonArrayValuesByArrayName(string json, string jsonArrayName)
        {
            List<string> values = new List<string>();
            string data = null;
            JArray jsonArray = JArray.Parse(json);
            for (int i = 0; i < jsonArray.Count; i++)
            {
                data = JObject.Parse(jsonArray[i].ToString()).ToString();
                string tokenValue = JObject.Parse(data).SelectToken(jsonArrayName).ToString();
                values.Add(tokenValue);
                Console.WriteLine("Json Path: [" + i + "]." + jsonArrayName + "\t Corresponding Value: " + tokenValue);
            }

            return values;
        }

        /// <summary>
        /// This method returns a particular arrayIndex value of a jsonArray token name from a json Response.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="jsonArrayName"></param>
        /// <param name="arrayIndex"></param>
        /// <returns></returns>
        public string GetJsonArrayValuesByArrayName(string json, string jsonArrayName, int arrayIndex)
        {
            string data = null;
            JArray jsonArray = JArray.Parse(json);

            data = JObject.Parse(jsonArray[arrayIndex].ToString()).ToString();
            string tokenValue = JObject.Parse(data).SelectToken(jsonArrayName).ToString();
            Console.WriteLine("Json Path: [" + arrayIndex + "]." + jsonArrayName + "\t Corresponding Value: " + tokenValue);

            return tokenValue;
        }

        
    }
}
