using Microsoft.Office.Interop.Excel;
using System;

namespace RestServicesAutomationFramework.DataReader
{
    class WeatherAPI_TestDataReader
    {
        Application excel = null;
        Workbook workbook = null;

        public string TestCaseId = "Empty";
        public string Description;
        public string Environment;
        public string API_EndPointURL;
        public string API_KEY;
        public string HeaderSet;
        public string Parameters;
        public string ExpectedStatusCode;
        public string JsonPath;
        public string StrDataItem1;
        public string StrDataItem2;
        public string StrDataItem3;
        public string StrDataItem4;
        public string StrDataItem5;


        public void GetData(string excelFilePath, string testCaseID)
        {
            excel = new Application();
            workbook = excel.Workbooks.Open(excelFilePath);

            //for the sheet named 'Data'
            Worksheet sheet = workbook.Sheets["Data"] as Worksheet;

            for (int row = 2; row <= 50; row++)
            {
                if (sheet.Cells[row, 1].Text.Trim() == (testCaseID))
                {
                    TestCaseId = sheet.Cells[row, 1].Text.Trim();
                    Description = sheet.Cells[row, 2].Text.Trim();
                    Environment = sheet.Cells[row, 3].Text.Trim();
                    API_EndPointURL = sheet.Cells[row, 4].Text.Trim();
                    API_KEY = sheet.Cells[row, 5].Text.Trim();
                    HeaderSet = sheet.Cells[row, 6].Text.Trim();
                    Parameters = sheet.Cells[row, 7].Text.Trim();
                    ExpectedStatusCode = sheet.Cells[row, 8].Text.Trim();
                    JsonPath = sheet.Cells[row, 9].Text.Trim();
                    StrDataItem1 = sheet.Cells[row, 10].Text.Trim();
                    StrDataItem2 = sheet.Cells[row, 11].Text.Trim();
                    StrDataItem3 = sheet.Cells[row, 12].Text.Trim();
                    StrDataItem4 = sheet.Cells[row, 13].Text.Trim();
                    StrDataItem5 = sheet.Cells[row, 14].Text.Trim();

                    break;
                }

            }
            workbook.Close();
            excel.Quit();

            if (TestCaseId == "Empty")
            {
                Console.WriteLine("TestcaseID: " + TestCaseId + " is not present in the Excel.");
                throw new Exception("Test Case Not Found in the Test Data Sheet!");
            }

        }
    }
}
