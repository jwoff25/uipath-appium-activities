using AndroidAutomator.Properties;
using System.Threading;
using System.Activities;
using System.ComponentModel;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.MultiTouch;

namespace AndroidAutomator.Activities
{
    [LocalizedDisplayName(nameof(Resources.SwipeActivityName))]
    [LocalizedDescription(nameof(Resources.SwipeActivityDesc))]
    public class Swipe : CodeActivity
    {
        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.StartXField))]
        [LocalizedDescription(nameof(Resources.StartXDesc))]
        [RequiredArgument]
        public InArgument<int> StartX { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.StartYField))]
        [LocalizedDescription(nameof(Resources.StartYDesc))]
        [RequiredArgument]
        public InArgument<int> StartY { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.EndXField))]
        [LocalizedDescription(nameof(Resources.EndXDesc))]
        [RequiredArgument]
        public InArgument<int> EndX { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.EndYField))]
        [LocalizedDescription(nameof(Resources.EndYDesc))]
        [RequiredArgument]
        public InArgument<int> EndY { get; set; }

        [LocalizedCategory(nameof(Resources.Options))]
        [LocalizedDisplayName(nameof(Resources.DelayField))]
        [LocalizedDescription(nameof(Resources.DelayDesc))]
        public InArgument<int> Delay { get; set; }

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
            int startx = StartX.Get(context);
            int starty = StartY.Get(context);
            int endx = EndX.Get(context);
            int endy = EndY.Get(context);
            int delay = Delay.Get(context);

            // Apply delay
            Thread.Sleep(delay);
           
            // Tap on the element
            new TouchAction(driver).Press(startx, starty).Wait(300).MoveTo(endx, endy).Release().Perform();
        }
    }
}
