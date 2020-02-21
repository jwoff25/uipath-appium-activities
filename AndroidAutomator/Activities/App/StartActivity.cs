using System;
using AndroidAutomator.Properties;
using System.Activities;
using System.Activities.Statements;
using System.ComponentModel;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;

namespace AndroidAutomator.Activities.App
{
    [LocalizedDisplayName(nameof(Resources.StartActivityActivityName))]
    [LocalizedDescription(nameof(Resources.StartActivityActivityDesc))]
    public class StartActivity : CodeActivity
    {
        [LocalizedCategory(nameof(Resources.App))]
        [LocalizedDisplayName(nameof(Resources.AppPackageField))]
        [LocalizedDescription(nameof(Resources.AppPackageDesc))]
        [RequiredArgument]
        public InArgument<string> AppPackage { get; set; }

        [LocalizedCategory(nameof(Resources.App))]
        [LocalizedDisplayName(nameof(Resources.AppActivityField))]
        [LocalizedDescription(nameof(Resources.AppActivityDesc))]
        [RequiredArgument]
        public InArgument<string> AppActivity { get; set; }

        [LocalizedCategory(nameof(Resources.AndroidDriver))]
        [LocalizedDisplayName(nameof(Resources.DriverField))]
        [LocalizedDescription(nameof(Resources.AndroidDriverDesc))]
        public InArgument<AndroidDriver<AndroidElement>> Driver { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            AndroidDriver<AndroidElement> driver;
            // Inherit driver from scope activity OR from input (if out of context)
            try
            {
                driver = context.DataContext.GetProperties()["Driver"].GetValue(context.DataContext) as AndroidDriver<AndroidElement>;
            }
            catch
            {
                driver = Driver.Get(context);
            }

            string appPackage = AppPackage.Get(context);
            string appActivity = AppActivity.Get(context);

            try
            {
                driver.StartActivity(appPackage, appActivity);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
