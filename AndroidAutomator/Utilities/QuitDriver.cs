﻿using System.Activities;
using AndroidAutomator.Properties;
using OpenQA.Selenium.Appium.Android;

namespace AndroidAutomator.Utilities
{
    [LocalizedDisplayName(nameof(Resources.QuitDriverActivityName))]
    [LocalizedDescription(nameof(Resources.QuitDriverActivityDesc))]
    public class QuitDriver : CodeActivity
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
            driver.Quit();
        }

    }
}
