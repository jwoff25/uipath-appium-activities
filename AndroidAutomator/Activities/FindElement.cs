using System.Activities;
using System.ComponentModel;
using OpenQA.Selenium.Appium.Android;

namespace AndroidAutomator.Activities
{

    public class FindElement : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Selector { get; set; }

        [Category("Input")]
        public InArgument<int> Index { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public SelectBy SelectType { get; set; }

        [Category("Options")]
        public bool MultipleElements { get; set; }

        [Category("Output")]
        public OutArgument<AndroidElement> Element { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Inherit driver from scope activity
            var driver = context.DataContext.GetProperties()["Driver"].GetValue(context.DataContext) as AndroidDriver<AndroidElement>;

            // Gather fields
            string selector = Selector.Get(context);
            int index = Index.Get(context);

            // Find and return element based on type
            AndroidElement element;
            if (!MultipleElements)
            {
                element = Helpers.GetElement(driver, SelectType, selector);
            }
            else
            {
                element = Helpers.GetElementFromList(driver, SelectType, selector, index);
            }
            Element.Set(context, element);
        }
    }
}
