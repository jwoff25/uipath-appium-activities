using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using AndroidAutomator.Activities;
using OpenQA.Selenium.Appium.Android;

namespace AndroidAutomator.Utilities
{

    class RunTest : NativeActivity
    {
        public InArgument<string> Filepath { get; set; }

        public FileType Type { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            // Inherit driver
            var driver = context.DataContext.GetProperties()["Driver"].GetValue(context.DataContext) as AndroidDriver<AndroidElement>;

            // Gather fields
            string path = Filepath.Get(context);

            switch(Type)
            {
                case FileType.Excel:
                    RunExcel(path, context, driver);
                    break;
                case FileType.JSON:
                    RunJSON(path, context, driver);
                    break;
            }
        }

        private void RunExcel(string path, NativeActivityContext context, AndroidDriver<AndroidElement> driver)
        {
            
        }

        private void RunJSON(string path, NativeActivityContext context, AndroidDriver<AndroidElement> driver)
        {

        }
    }
}
