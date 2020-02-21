using System;
using AndroidAutomator.Properties;
using System.Activities;
using System.Activities.Statements;
using System.ComponentModel;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;

namespace AndroidAutomator.Activities.Browser
{
    [LocalizedDisplayName(nameof(Resources.GoToURLActivityName))]
    [LocalizedDescription(nameof(Resources.GoToURLActivityDesc))]
    public class GoToURL : CodeActivity
    {
        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.URLFieldName))]
        [LocalizedDescription(nameof(Resources.URLFieldDesc))]
        [RequiredArgument]
        public InArgument<string> URL { get; set; }

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

            // Receive fields
            string url = URL.Get(context);

            // Try to get URL
            try
            {
                var nav = driver.Navigate();
                nav.GoToUrl(url);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
