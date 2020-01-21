using System.IO;
using System.Threading;
using System.Activities;
using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using AndroidAutomator.Properties;

namespace AndroidAutomator.Activities
{
    [LocalizedDisplayName(nameof(Resources.TakeScreenshotActivityName))]
    [LocalizedDescription(nameof(Resources.TakeScreenshotActivityDesc))]
    public class TakeScreenshot : CodeActivity
    {
        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.FilenameField))]
        [LocalizedDescription(nameof(Resources.FilenameDesc))]
        public InArgument<string> Filename { get; set; }

        [LocalizedCategory(nameof(Resources.Options))]
        [LocalizedDisplayName(nameof(Resources.DelayField))]
        [LocalizedDescription(nameof(Resources.DelayDesc))]
        public InArgument<int> Delay { get; set; } = 1000;

        [LocalizedCategory(nameof(Resources.AndroidDriver))]
        [LocalizedDisplayName(nameof(Resources.DriverField))]
        [LocalizedDescription(nameof(Resources.AndroidDriverDesc))]
        public InArgument<AndroidDriver<AndroidElement>> Driver { get; set; }

        // Add wait for element 

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
            string filename = Filename.Get(context) ?? "capture";
            int delay = Delay.Get(context);

            // Set up path
            string defaultPath = driver.Capabilities.GetCapability(AndroidMobileCapabilityType.AndroidScreenshotPath) as string;
            string fullPath = Path.Combine(defaultPath, filename + ".png");

            Thread.Sleep(delay);

            // Take screenshot
            Screenshot s = driver.GetScreenshot();
            s.SaveAsFile(fullPath, ScreenshotImageFormat.Png);
        }
    }
}
