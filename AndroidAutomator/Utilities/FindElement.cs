using System.Activities;
using System.ComponentModel;
using OpenQA.Selenium.Appium.Android;

namespace AndroidAutomator.Utilities
{

    public class FindElement : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Selector { get; set; }

        [Category("Input")]
        public InArgument<AndroidElement> Parent { get; set; }

        [Category("Input")]
        public InArgument<int> Index { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public SelectBy SelectType { get; set; }

        [Category("Options")]
        public bool MultipleElements { get; set; }

        [Category("Output")]
        public OutArgument<AndroidElement> Element { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            // If neither a selector nor an element is entered
            if (Selector == null && Parent == null)
            {
                metadata.AddValidationError("Please fill in all of the following fields: Selector, Parent.");
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
