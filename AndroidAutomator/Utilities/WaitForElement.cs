using System;
using System.Activities;
using System.ComponentModel;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Appium.Android;

namespace AndroidAutomator.Utilities
{

    public sealed class WaitForElement : CodeActivity
    {
        [Category("Input")]
        public InArgument<string> Selector { get; set; }

        [Category("Input")]
        public InArgument<AndroidElement> Element { get; set; }

        [Category("Input")]
        public InArgument<int> Index { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public SelectBy SelectType { get; set; }

        [Category("Options")]
        public bool MultipleElements { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Inherit driver from scope activity
            var driver = context.DataContext.GetProperties()["Driver"].GetValue(context.DataContext) as AndroidDriver<AndroidElement>;

            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan());
        }
    }
}
