using AndroidAutomator.Properties;
using System.Activities;
using System.ComponentModel;
using OpenQA.Selenium.Appium.Android;
using System.Collections.Generic;

namespace AndroidAutomator.Utilities
{
    [LocalizedDisplayName(nameof(Resources.ExecuteScriptActivityName))]
    [LocalizedDescription(nameof(Resources.ExecuteScriptActivityDesc))]
    public class ExecuteScript : CodeActivity
    {
        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.CommandNameField))]
        [LocalizedDescription(nameof(Resources.CommandNameDesc))]
        [RequiredArgument]
        public InArgument<string> Command { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.ArgumentsField))]
        [LocalizedDescription(nameof(Resources.ArgumentsDesc))]
        [RequiredArgument]
        public InArgument<Dictionary<string, string>> Arguments { get; set; }

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
            
            // Gather fields
            string command = Command.Get(context);
            Dictionary<string, string> args = Arguments.Get(context);

            // Execute Script
            driver.ExecuteScript(command, args);
        }
    }
}
