using NUnit.Core.Extensibility;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using RelevantCodes.ExtentReports;
using RestServicesAutomationFramework.Logger;
using RestServicesAutomationFramework.Util.RestService;
using RestServicesAutomationFramework.Util.SoapService;
using RestServicesAutomationFramework.Util.WebAutomation;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using static RestServicesAutomationFramework.Logger.FileLogger;

namespace RestServicesAutomationFramework.Util
{
    class Common : WebAutomationFramework
    {

        #region Fields
        private RestFramework rest = new RestFramework();
        private SoapFramework soap = new SoapFramework();
        private FileLogger logObject = null;
        #endregion

        #region Getters

        public RestFramework getRestFrameworkInstance()
        {
            return rest;
        }
        public SoapFramework getSoapFrameworkInstance()
        {
            return soap;
        }

        #endregion

        #region REST Service Common Methods

        public string NormalGetOperation(string TestCaseId, string endPointUrl)
        {
            return rest.NormalGetOperation(TestCaseId, endPointUrl);
        }

        public string OAuth_2_GetAccessToken(string testCaseId, string endPointUrl, string headerSet, string client_id, string client_secret)
        {
            return rest.OAuth_2_GetAccessToken(testCaseId, endPointUrl, headerSet, client_id, client_secret);
        }

        public string Normal_Post_Put_Operation(string testCaseId, string endPointUrl, Method httpMethod, string headerSet, string payload, string accessToken)
        {
            return rest.Normal_Post_Put_Operation(testCaseId, endPointUrl, httpMethod, headerSet, payload, accessToken);
        }

        public string GetSpecificTokenValue(string jsonResponse, string jsonPath)
        {
            return rest.GetSpecificTokenValue(jsonResponse, jsonPath);
        }

        public string TryToGetSpecificTokenValue(string jsonResponse, string jsonPath)
        {
            return rest.TryToGetSpecificTokenValue(jsonResponse, jsonPath);
        }

        public List<string> RetrieveMultipleTokens(string jsonResponse, string jsonPath)
        {
            return rest.RetrieveMultipleTokens(jsonResponse, jsonPath);
        }

        public string GenericHttpOperation_OAuth(string testCaseID, string endPointUrl, string headerSet, string parameters, Method httpMethod, string requestPayload, string accessToken)
        {
            return rest.GenericHttpOperation_OAuth(testCaseID, endPointUrl, headerSet, parameters, httpMethod, requestPayload, accessToken);
        }

        public HttpStatusCode getResponseStatus()
        {
            return rest.getResponseStatus();
        }

        public IRestResponse getRestResponseInstance()
        {
            return rest.getRestResponseInstance();
        }

        public List<string> GetJsonArrayValuesByArrayName(string json, string jsonArrayName)
        {
            return rest.GetJsonArrayValuesByArrayName(json, jsonArrayName);
        }

        public string GetJsonArrayValuesByArrayName(string json, string jsonArrayName, int arrayIndex)
        {
            return rest.GetJsonArrayValuesByArrayName(json, jsonArrayName, arrayIndex);
        }

        #endregion

        #region File Logger

        public void setFileLoggerInstance(FileLogger fileLogger)
        {
            this.logObject = fileLogger;
        }

        //public void log(string message)
        //{
        //    logObject.Log(message);
        //}

        public void WriteFormattedLog(LogLevel level, string text)
        {
            logObject.WriteFormattedLog(level, text);
        }

        #endregion

        #region ExtentReports

        private static ExtentReports extent;
        private static ExtentTest test;

        public static void StartReport(string testCaseID)
        {
            //To obtain the current solution path/project path
            string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath = pth.Substring(0, pth.LastIndexOf("bin"));
            string projectPath = new Uri(actualPath).LocalPath;

            //Append the html report file to current project path
            string reportPath = projectPath + "Reports\\" + testCaseID + "_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss") + ".html";

            //Boolean value for replacing exisisting report
            extent = new ExtentReports(reportPath, true);

            //Add QA system info to html report
            extent.AddSystemInfo("Host Name", "DemoHostName").AddSystemInfo("Environment", "QAEnvironment").AddSystemInfo("Username", "YourUserName");

            //Adding config.xml file
            extent.LoadConfig(projectPath + "Extent-Config.xml");

        }

        public static void StartExtentTest(string testToStart)
        {
            test = extent.StartTest(testToStart);
        }

        public static void FailureCheck()
        {

            //StackTrace details for failed Testcases
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = TestContext.CurrentContext.Result.StackTrace;
            var errorMessage = TestContext.CurrentContext.Result.Message;

            if (status == TestStatus.Failed)
            {
                test.Log(LogStatus.Fail, "[" + status + "] " + errorMessage + " [StackTrace] :" + stackTrace);
            }
            else if (status == TestStatus.Passed)
            {
                test.Log(LogStatus.Pass, "[" + status + "]");
                test.Log(LogStatus.Pass, "No of Assertions Passed: " + TestContext.CurrentContext.AssertCount);
            }
            else if (status == TestStatus.Skipped)
            {
                test.Log(LogStatus.Pass, "" + status);
            }
            else if (status == TestStatus.Skipped)
            {
                test.Log(LogStatus.Pass, "" + status);
            }
            EndExtentTest();
        }

        public static void EndExtentTest()
        {
            //End test report
            extent.EndTest(test);
        }

        public static void EndReport()
        {
            //End Report
            extent.Flush();
            extent.Close();
        }

        public void log(LogStatus logStatus, string message)
        {
            test.Log(logStatus, message);
        }

        public void log(string message)
        {
            log(LogStatus.Info, message);
        }

        #endregion
        
        #region Other Methods
        /// <summary>
        /// This method is to read a file and return the content as a string.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string ReadFile(string filePath)
        {
            string fileContent = null;
            using (StreamReader sr = new StreamReader(filePath))
            {
                // Read the stream to a string, and write the string to the console.
                fileContent = sr.ReadToEnd();
                Console.WriteLine(fileContent);
            }

            return fileContent;
        }

        /// <summary>
        /// This method takes a string along with a delimeter and returns the splitted array of the same string.
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        public string[] SplitToArray(string sentence, char delimeter)
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
        #endregion

    }
}
