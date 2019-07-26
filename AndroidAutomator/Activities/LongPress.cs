using System.Threading;
using System.Activities;
using System.ComponentModel;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.MultiTouch;

namespace AndroidAutomator.Activities
{
    public class LongPress : CodeActivity
    {
        [Category("Input")]
        public InArgument<string> Selector { get; set; }

        [Category("Input")]
        public InArgument<AndroidElement> Element { get; set; }

        [Category("Input")]
        public InArgument<int> Index { get; set; }

        [Category("Options")]
        public bool MultipleElements { get; set; }

        [Category("Options")]
        public bool UseCoordinates { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public SelectBy SelectType { get; set; }

        [Category("Options")]
        public InArgument<int> Delay { get; set; }

        [Category("Coordinates")]
        public InArgument<int> X { get; set; }

        [Category("Coordinates")]
        public InArgument<int> Y { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            // Make it so you can't have both options selected
            if (UseCoordinates && MultipleElements)
            {
                metadata.AddValidationError("Both options cannot be selected.");
            }
            // Coordinates must be provided if UseCoordinates is true
            else if (UseCoordinates)
            {
                if (X == null || Y == null)
                {
                    metadata.AddValidationError("Please provide both X and Y coordinates.");
                }
            }
            // Selector must be provided if coordinates are not being used
            else
            {
                if (Selector == null && Element == null)
                {
                    metadata.AddValidationError("Please fill in one of the following fields: Selector, Element.");
                }
                if (MultipleElements)
                {
                    if (Index == null)
                    {
                        metadata.AddValidationError("Please provide an index.");
                    }
                }
            }
        }

        protected override void Execute(CodeActivityContext context)
        {
            // Inherit driver from scope activity
            var driver = context.DataContext.GetProperties()["Driver"].GetValue(context.DataContext) as AndroidDriver<AndroidElement>;

            // Get delay
            int delay = Delay.Get(context);

            // If coordinates are to be used
            if (UseCoordinates)
            {
                int x = X.Get(context);
                int y = Y.Get(context);

                // Apply delay
                Thread.Sleep(delay);

                // Tap by coordinates
                new TouchAction(driver).LongPress(x, y).Perform();
            }
            // Else find and tap element
            else
            {
                // Receive fields
                string selector = Selector.Get(context);
                int index = Index.Get(context);
                AndroidElement inputElement = Element.Get(context);

                // Logic to determine target element
                AndroidElement element;
                // User entered an element
                if (inputElement != null)
                {
                    element = inputElement;
                }
                // User didn't enter an element
                else
                {
                    // Get element depending on SelectBy enum
                    if (!MultipleElements)
                    {
                        element = Helpers.GetElement(driver, SelectType, selector);
                    }
                    else
                    {
                        element = Helpers.GetElementFromList(driver, SelectType, selector, index);
                    }
                }

                // Apply delay
                Thread.Sleep(delay);

                // Tap on the element
                new TouchAction(driver).LongPress(element).Perform();
            }
        }
    }
}
