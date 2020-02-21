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
    [LocalizedDisplayName(nameof(Resources.LaunchAppActivityName))]
    [LocalizedDescription(nameof(Resources.LaunchAppActivityDesc))]
    public class LaunchApp : CodeActivity
    {

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

            try
            {
                driver.LaunchApp();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
