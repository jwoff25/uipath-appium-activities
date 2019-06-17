using System;
using System.Activities;
using System.ComponentModel;
using OpenQA.Selenium.Appium.Android;

namespace AndroidAutomator.Activities
{

    public class EnterText : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Selector { get; set; }

        [Category("Input")]
        public InArgument<AndroidElement> Element { get; set; }

        [Category("Input")]
        public InArgument<int> Index { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public SelectBy SelectType { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Text { get; set; }

        [Category("Options")]
        public bool MultipleElements { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            // If neither a selector nor an element is entered
            if (Selector == null && Element == null)
            {
                metadata.AddValidationError("You must fill in one of the following fields: Selector, Element.");
            }
            // If both selector and element fields are filled
            if (Selector != null && Element != null)
            {
                metadata.AddValidationError("Selector and Element fields cannot both be filled.");
            }
            // If multiple elements is selected, an index must be provided
            if (MultipleElements)
            {
                if (Index == null)
                {
                    metadata.AddValidationError("You must provide an index.");
                }
            }
        }

        protected override void Execute(CodeActivityContext context)
        {
            // Inherit driver from scope activity
            var driver = context.DataContext.GetProperties()["Driver"].GetValue(context.DataContext) as AndroidDriver<AndroidElement>;

            // Receive fields
            string text = Text.Get(context);
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

            // Try to send text
            try
            {
                element.SendKeys(text);
                // Close keyboard after sending
                driver.HideKeyboard();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to send text. " + e);
            }
        }
    }
}
