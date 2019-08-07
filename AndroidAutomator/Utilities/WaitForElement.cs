using System;
using System.Activities;
using System.ComponentModel;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium;
using AndroidAutomator.Properties;

namespace AndroidAutomator.Utilities
{
    [LocalizedDisplayName(nameof(Resources.WaitForElementActivityName))]
    [LocalizedDescription(nameof(Resources.WaitForElementActivityDesc))]
    public class WaitForElement : CodeActivity
    {
        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.SelectorField))]
        [LocalizedDescription(nameof(Resources.SelectorDesc))]
        [RequiredArgument]
        public InArgument<string> Selector { get; set; }

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
        [LocalizedDisplayName(nameof(Resources.WaitTimeField))]
        [LocalizedDescription(nameof(Resources.ExplicitWaitDesc))]
        public InArgument<int> Time { get; set; } = 10;

        [LocalizedCategory(nameof(Resources.Options))]
        [LocalizedDisplayName(nameof(Resources.MultipleElementsField))]
        [LocalizedDescription(nameof(Resources.MultipleElementsDesc))]
        public bool MultipleElements { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
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

            // Gather fields
            string selector = Selector.Get(context);
            int index = Index.Get(context);
            int time = Time.Get(context);

            // Create a wait object
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, time));

            // Wait until element can be found
            wait.Until(condition =>
            {
                try
                {
                    // Get element depending on SelectBy enum
                    if (!MultipleElements)
                    {
                        return Helpers.GetElement(driver, SelectType, selector).Displayed;
                    }
                    else
                    {
                        return Helpers.GetElementFromList(driver, SelectType, selector, index).Displayed;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
            
        }
    }
}
