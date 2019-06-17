using System.Threading;
using System.Activities;
using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace AndroidAutomator.Activities
{
    public class TakeScreenshot : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Filename { get; set; }

        [Category("Options")]
        public InArgument<int> Delay { get; set; } = 1000;

        protected override void Execute(CodeActivityContext context)
        {
            // Inherit driver from scope activity
            var driver = context.DataContext.GetProperties()["Driver"].GetValue(context.DataContext) as AndroidDriver<AndroidElement>;

            // Receive fields
            string filename = Filename.Get(context);
            int delay = Delay.Get(context);

            // Apply delay
            Thread.Sleep(delay);

            // Tap on the element
            Screenshot s = driver.GetScreenshot();
            s.SaveAsFile(filename + ".png", ScreenshotImageFormat.Png);
        }
    }
}
