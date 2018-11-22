using NUnit.Framework;
using NUnit.Framework.Internal;
using Oracle.ManagedDataAccess.Client;
using RelevantCodes.ExtentReports;
using RestServicesAutomationFramework.DataReader;
using RestServicesAutomationFramework.Util;
using System;
using System.Collections.Generic;

namespace RestServicesAutomationFramework.Test.LOB.WeatherDepartment
{
    [TestFixture]
    class NewWeatherTest : Common
    {
        WeatherAPI_TestDataReader data = new WeatherAPI_TestDataReader();

        //----------------------------------------**** SETUP REGION ****------------------------------------------------
        #region One Time Setup

        [OneTimeSetUp]
        public void OneTimeConfigurations()
        {
            StartReport(TestContext.CurrentContext.Test.Name);
        }
        #endregion

        #region Setup
        public void Setup(string testCaseID)
        {
            StartExtentTest(testCaseID);
            log("TestCaseName: "+testCaseID);
            log(LogStatus.Info, "TestCase Started.");
            data.GetData(@"C:\Users\rohit_knw2paf\Desktop\testingAutomation\RestServicesAutomationFramework\RestServicesAutomationFramework\Resources\WeatherAPI_TestData.xlsx", testCaseID);
            log("TestCase Description: " + data.Description);
        }
        #endregion
        //--------------------------------------------*** TEST REGION ***------------------------------------------------
        #region Test
        [TestCase("TC001_WeatherTest_Location_NewYork")]
        [TestCase("TC002_WeatherTest_Location_Hyderabad")]
        public void New_WeatherAPI_Tests(string testCaseID)
        {
            Setup(testCaseID);
            string api_parameter = data.Parameters.Replace("#", data.StrDataItem1).Replace("$", data.API_KEY);
            string weatherApiJsonResponse = GenericHttpOperation_OAuth(testCaseID, data.API_EndPointURL, data.HeaderSet, api_parameter, RestSharp.Method.GET, "", "");
            string statusCode = getResponseStatus().ToString();
            Console.WriteLine(statusCode);

            log("API Response Status Code:" + statusCode);
            log("API Response JSON:" + weatherApiJsonResponse);
            log("Json Path: "+data.JsonPath);

            string locationNameFromJsonResponse = GetSpecificTokenValue(weatherApiJsonResponse, data.JsonPath);
            Assert.AreEqual(locationNameFromJsonResponse, data.StrDataItem1);
            log("Expected Location: "+data.StrDataItem1);
            log("Actual Location value got from json response: " + locationNameFromJsonResponse);
            Assert.True(true);
            List<string> weatherValues = RetrieveMultipleTokens(weatherApiJsonResponse, "weather[*].id");
            string myError = null;
            if (myError == null && testCaseID.Contains("Hyderabad"))
            {
                throw new NullReferenceException("myError variable is null.");
            }
            Console.WriteLine("The above line should throw null pointer exception");


            //Method Testing
            string dummyStringJson = ReadFile(@"C:\Users\rohit_knw2paf\Desktop\testingAutomation\RestServicesAutomationFramework\RestServicesAutomationFramework\Resources\dummy.json");
            List<string> dummyJsonTokenValues = RetrieveMultipleTokens(dummyStringJson, "store.book[*].author");


            string oradb = "Data Source=(DESCRIPTION =" + "(ADDRESS = (PROTOCOL = TCP)(HOST = localhost" +
                ")(PORT = 1521))" + "(CONNECT_DATA =" + "(SERVER =)" + "(SERVICE_NAME = orcl)));" + "User Id= hr;Password=hr;";
            OracleConnection conn = new OracleConnection(oradb);
            conn.Open();
            Console.WriteLine("Connected to Oracle" + conn.ServerVersion);
            // Close and Dispose OracleConnection object  
            conn.Close();
            conn.Dispose();
            Console.WriteLine("Disconnected");
        }
        #endregion

        #region Common Methods for TestCases
        #endregion

        //--------------------------------------------------------*** TEAR DOWN REGION ***----------------------------------------
        #region Dispose
        [TearDown]
        public void Dispose()
        {
            FailureCheck();
        }
        #endregion

        #region One time Tear Down
        [OneTimeTearDown]
        public void OneTimeDispose()
        {
            EndReport();
        }
        #endregion
    }
}
