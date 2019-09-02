using AndroidAutomator.Properties;
using System.Activities;
using System.ComponentModel;
using OpenQA.Selenium.Appium.Android;

namespace AndroidAutomator.Utilities
{
    [LocalizedDisplayName(nameof(Resources.FindElementActivityName))]
    [LocalizedDescription(nameof(Resources.FindElementActivityDesc))]
    public class FindElement : CodeActivity
    {
        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.SelectorField))]
        [LocalizedDescription(nameof(Resources.SelectorDesc))]
        [RequiredArgument]
        public InArgument<string> Selector { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.ParentField))]
        [LocalizedDescription(nameof(Resources.ParentDesc))]
        public InArgument<AndroidElement> Parent { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.IndexField))]
        [LocalizedDescription(nameof(Resources.IndexDesc))]
        public InArgument<int> Index { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.SelectTypeField))]
        [LocalizedDescription(nameof(Resources.SelectTypeDesc))]
        [RequiredArgument]
        public SelectBy SelectType { get; set; }

        [LocalizedCategory(nameof(Resources.Options))]
        [LocalizedDisplayName(nameof(Resources.MultipleElementsField))]
        [LocalizedDescription(nameof(Resources.MultipleElementsDesc))]
        public bool MultipleElements { get; set; }

        [LocalizedCategory(nameof(Resources.Options))]
        [LocalizedDisplayName(nameof(Resources.ElementField))]
        [LocalizedDescription(nameof(Resources.ElementDesc))]
        public OutArgument<AndroidElement> Element { get; set; }

        [LocalizedCategory(nameof(Resources.AndroidDriver))]
        [LocalizedDisplayName(nameof(Resources.DriverField))]
        [LocalizedDescription(nameof(Resources.AndroidDriverDesc))]
        public InArgument<AndroidDriver<AndroidElement>> Driver { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            // If neither a selector nor an element is entered
            if (Selector == null && Parent == null)
            {
                metadata.AddValidationError("Please fill in either/both of the following fields: Selector, Parent.");
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

            // Gather fields
            string selector = Selector.Get(context);
            int index = Index.Get(context);
            AndroidElement parent = Parent.Get(context);

            // Logic to determine target element
            AndroidElement element;
            // User entered an element
            if (parent != null)
            {
                // Get child element based on parent
                if (!MultipleElements)
                {
                    element = Helpers.GetChildElement(parent, SelectType, selector);
                }
                else
                {
                    element = Helpers.GetChildElementFromList(parent, SelectType, selector, index);
                }
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
            
            // Return element
            Element.Set(context, element);
        }
    }
}
