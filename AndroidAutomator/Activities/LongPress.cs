using AndroidAutomator.Properties;
using System.Threading;
using System.Activities;
using System.ComponentModel;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.MultiTouch;

namespace AndroidAutomator.Activities
{
    [LocalizedDisplayName(nameof(Resources.LongPressActivityName))]
    [LocalizedDescription(nameof(Resources.LongPressActivityDesc))]
    public class LongPress : CodeActivity
    {
        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.SelectorField))]
        [LocalizedDescription(nameof(Resources.SelectorDesc))]
        public InArgument<string> Selector { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.ElementField))]
        [LocalizedDescription(nameof(Resources.ElementDesc))]
        public InArgument<AndroidElement> Element { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.IndexField))]
        [LocalizedDescription(nameof(Resources.IndexDesc))]
        public InArgument<int> Index { get; set; }

        [LocalizedCategory(nameof(Resources.Options))]
        [LocalizedDisplayName(nameof(Resources.MultipleElementsField))]
        [LocalizedDescription(nameof(Resources.MultipleElementsDesc))]
        public bool MultipleElements { get; set; }

        [LocalizedCategory(nameof(Resources.Options))]
        [LocalizedDisplayName(nameof(Resources.UseCoordinatesField))]
        [LocalizedDescription(nameof(Resources.UseCoordinatesDesc))]
        public bool UseCoordinates { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.SelectTypeField))]
        [LocalizedDescription(nameof(Resources.SelectTypeDesc))]
        [RequiredArgument]
        public SelectBy SelectType { get; set; }

        [LocalizedCategory(nameof(Resources.Options))]
        [LocalizedDisplayName(nameof(Resources.DelayField))]
        [LocalizedDescription(nameof(Resources.DelayDesc))]
        public InArgument<int> Delay { get; set; }

        [LocalizedCategory(nameof(Resources.Coordinates))]
        [LocalizedDisplayName(nameof(Resources.XField))]
        [LocalizedDescription(nameof(Resources.XDesc))]
        public InArgument<int> X { get; set; }

        [LocalizedCategory(nameof(Resources.Coordinates))]
        [LocalizedDisplayName(nameof(Resources.YField))]
        [LocalizedDescription(nameof(Resources.YField))]
        public InArgument<int> Y { get; set; }

        [LocalizedCategory(nameof(Resources.AndroidDriver))]
        [LocalizedDisplayName(nameof(Resources.DriverField))]
        [LocalizedDescription(nameof(Resources.AndroidDriverDesc))]
        public InArgument<AndroidDriver<AndroidElement>> Driver { get; set; }

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
