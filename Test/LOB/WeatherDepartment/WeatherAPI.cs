using log4net;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using RelevantCodes.ExtentReports;
using RestServicesAutomationFramework.DataReader;
using RestServicesAutomationFramework.Logger;
using RestServicesAutomationFramework.Util;
using System;
using System.Collections.Generic;
using static RestServicesAutomationFramework.Logger.FileLogger;

namespace RestServicesAutomationFramework.Test.LOB.WeatherDepartment
{
    [TestFixture]
    
    class WeatherAPI : Common
    {
        WeatherAPI_TestDataReader data = new WeatherAPI_TestDataReader();
        //ILog logger = null;
        ExtentTest extentLog = null;

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
            extentLog = ExtentReporting.test;

            data.GetData(@"C:\Users\rohit_knw2paf\Desktop\testingAutomation\RestServicesAutomationFramework\RestServicesAutomationFramework\Resources\WeatherAPI_TestData.xlsx", testCaseID);
            setFileLoggerInstance(new FileLogger(testCaseID));
            //logger = Log4NetHelper.GetLogger(typeof(WeatherAPI), @"C:\Users\rohit_knw2paf\Desktop\testingAutomation\RestServicesAutomationFramework\RestServicesAutomationFramework\Output\", testCaseID);
        }
        #endregion

        #region Test
        [TestCase("TC001_WeatherTest_Location_NewYork")]
        [TestCase("TC002_WeatherTest_Location_Hyderabad")]
        public void WeatherAPI_Tests(string testCaseID)
        {
            Setup(testCaseID);
            string api_parameter = data.Parameters.Replace("#", data.StrDataItem1).Replace("$", data.API_KEY);
            string weatherApiJsonResponse = GenericHttpOperation_OAuth(testCaseID, data.API_EndPointURL, data.HeaderSet, api_parameter, RestSharp.Method.GET, "", "");
            string statusCode = getResponseStatus().ToString();
            Console.WriteLine(statusCode);

            //logger.Debug("This is debug message");
            //logger.Info("This is info message");
            //logger.Warn("This is warn message");
            //logger.Error("This is error message");
            //logger.Fatal("This is fatal message");

            //log(statusCode);
            //log(weatherApiJsonResponse);

            extentLog.Log(LogStatus.Info,"TestCase Started.");
            extentLog.Log(LogStatus.Info, "API Response Status Code:"+statusCode);
            extentLog.Log(LogStatus.Info, "API Response JSON:" + weatherApiJsonResponse);


            string locationNameFromJsonResponse = GetSpecificTokenValue(weatherApiJsonResponse, data.JsonPath);
            //validation of the Json Response Parameter with the Excel Expected Value
            Assert.AreEqual(locationNameFromJsonResponse, data.StrDataItem1);
            //Assert.AreEqual(locationNameFromJsonResponse, "wrong Value to cause an error.");
            WriteFormattedLog(LogLevel.INFO, "Location from the json Response: "+ locationNameFromJsonResponse);
            Assert.True(true);
            List<string> weatherValues = RetrieveMultipleTokens(weatherApiJsonResponse, "weather[*].id");
            string myError = null;
            if (myError==null && testCaseID.Contains("Hyderabad"))
            {
                throw new NullReferenceException("myError variable is null.");
            }
            Console.WriteLine("The above line should throw null pointer exception");

        }
        #endregion

        #region Common Methods for TestCases
        
        #endregion

        #region Dispose
        [TearDown]
        public void Dispose()
        {
            //if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
            //{
            //    Console.WriteLine(TestContext.CurrentContext.Result.Outcome);
            //    Console.WriteLine(TestContext.CurrentContext.Result.Message);
            //    Console.WriteLine(TestContext.CurrentContext.Result.StackTrace);
            //}

            FailureCheck();
        }
        #endregion

        #region One time Tear Down
        [OneTimeTearDown]
        public void OneTimeDispose()
        {
            ExtentReporting.EndReport();
        }
        #endregion
    }
}
