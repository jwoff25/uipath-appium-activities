using System.Threading;
using System.Activities;
using System.ComponentModel;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.MultiTouch;

namespace AndroidAutomator.Activities
{
    public class Swipe : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<int> StartX { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<int> StartY { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<int> EndX { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<int> EndY { get; set; }

        [Category("Options")]
        public InArgument<int> Delay { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Inherit driver from scope activity
            var driver = context.DataContext.GetProperties()["Driver"].GetValue(context.DataContext) as AndroidDriver<AndroidElement>;

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
