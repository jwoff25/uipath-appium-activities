using System;
using AndroidAutomator.Properties;
using System.Activities;
using System.ComponentModel;
using OpenQA.Selenium.Appium.Android;

namespace AndroidAutomator.Activities
{
    [LocalizedDisplayName(nameof(Resources.EnterTextActivityName))]
    [LocalizedDescription(nameof(Resources.EnterTextActivityDesc))]
    public class EnterText : CodeActivity
    {
        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.SelectorField))]
        [LocalizedDescription(nameof(Resources.SelectorDesc))]
        [RequiredArgument]
        public InArgument<string> Selector { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.ElementField))]
        [LocalizedDescription(nameof(Resources.ElementDesc))]
        public InArgument<AndroidElement> Element { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.IndexField))]
        [LocalizedDescription(nameof(Resources.IndexDesc))]
        public InArgument<int> Index { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.SelectTypeField))]
        [LocalizedDescription(nameof(Resources.SelectTypeDesc))]
        [RequiredArgument]
        public SelectBy SelectType { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.TextField))]
        [LocalizedDescription(nameof(Resources.EnterTextDesc))]
        [RequiredArgument]
        public InArgument<string> Text { get; set; }

        [LocalizedCategory(nameof(Resources.Options))]
        [LocalizedDisplayName(nameof(Resources.MultipleElementsField))]
        [LocalizedDescription(nameof(Resources.MultipleElementsDesc))]
        public bool MultipleElements { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            // If neither a selector nor an element is entered
            if (Selector == null && Element == null)
            {
                metadata.AddValidationError("Please fill in one of the following fields: Selector, Element.");
            }
            // If multiple elements is selected, an index must be provided
            if (MultipleElements)
            {
                if (Index == null)
                {
                    metadata.AddValidationError("Please provide an index.");
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
