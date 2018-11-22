using NUnit.Framework;
using NUnit.Framework.Interfaces;
using RelevantCodes.ExtentReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestServicesAutomationFramework.Logger
{
    [TestFixture]
    class ExtentReporting
    {
        public static ExtentReports extent;

        public static ExtentTest test;

        public static void StartReport(string NunitName_TestCaseMethodName)
        {
            //To obtain the current solution path/project path

            string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath = pth.Substring(0, pth.LastIndexOf("bin"));
            string projectPath = new Uri(actualPath).LocalPath;

            //Append the html report file to current project path
            string reportPath = projectPath + "Reports\\"+ NunitName_TestCaseMethodName + "_"+DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss")+".html";

            //Boolean value for replacing exisisting report
            extent = new ExtentReports(reportPath, true);

            //Add QA system info to html report
            extent.AddSystemInfo("Host Name", "YourHostName").AddSystemInfo("Environment", "YourQAEnvironment").AddSystemInfo("Username", "YourUserName");

            //Adding config.xml file
            extent.LoadConfig(projectPath + "Extent-Config.xml"); //Get the config.xml file from http://extentreports.com

        }

        public static void StartExtentTest(string testToStart)
        {
            test = extent.StartTest(testToStart);
        }

        //[TearDown]

        public static void AfterClass(string testCaseDescription)
        {

            //StackTrace details for failed Testcases

            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = "" + TestContext.CurrentContext.Result.StackTrace + "";
            var errorMessage = TestContext.CurrentContext.Result.Message;

            if (status == TestStatus.Failed)
            {
                test.Log(LogStatus.Fail, "Description: "+ testCaseDescription + " ["+status +"] "+ errorMessage+ " [StackTrace] :"+ stackTrace);
                test.Log(LogStatus.Fail, "Exception Came!");
            }
            else if (status == TestStatus.Passed)
            {
                test.Log(LogStatus.Pass, "Description: " + testCaseDescription + " [" + status + "] No of Assertions Passed: "+ TestContext.CurrentContext.AssertCount);
            }
            else if (status == TestStatus.Skipped)
            {
                test.Log(LogStatus.Pass, ""+status);
            }
            else if (status == TestStatus.Skipped)
            {
                test.Log(LogStatus.Pass, "" + status);
            }

            //End test report
            extent.EndTest(test);
        }

        // [OneTimeTearDown]

        public static void EndReport()
        {
            //End Report
            extent.Flush();
            extent.Close();
        }
    }
}
