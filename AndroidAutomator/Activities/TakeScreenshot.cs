using System.IO;
using System.Threading;
using System.Activities;
using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace AndroidAutomator.Activities
{
    public class TakeScreenshot : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Filename { get; set; }

        [Category("Options")]
        public InArgument<int> Delay { get; set; } = 1000;

        // Add wait for element 

        protected override void Execute(CodeActivityContext context)
        {
            // Inherit driver from scope activity
            var driver = context.DataContext.GetProperties()["Driver"].GetValue(context.DataContext) as AndroidDriver<AndroidElement>;

            // Receive fields
            string filename = Filename.Get(context);
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
