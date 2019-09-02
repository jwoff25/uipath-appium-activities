using System.Activities;
using AndroidAutomator.Properties;
using System.ComponentModel;
using OpenQA.Selenium.Appium.Android;

namespace AndroidAutomator.Activities
{
    [LocalizedDisplayName(nameof(Resources.ToolbarActivityName))]
    [LocalizedDescription(nameof(Resources.ToolbarActivityDesc))]
    public class AndroidToolbarButton : CodeActivity
    {
        [LocalizedCategory(nameof(Resources.Type))]
        [LocalizedDisplayName(nameof(Resources.KeyCodeField))]
        [LocalizedDescription(nameof(Resources.KeyCodeDesc))]
        [RequiredArgument]
        public AndroidKeyCodes KeyCode { get; set; }

        [LocalizedCategory(nameof(Resources.AndroidDriver))]
        [LocalizedDisplayName(nameof(Resources.DriverField))]
        [LocalizedDescription(nameof(Resources.AndroidDriverDesc))]
        public InArgument<AndroidDriver<AndroidElement>> Driver { get; set; }

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

            switch (KeyCode)
            {
                case AndroidKeyCodes.Back:
                    driver.PressKeyCode(AndroidKeyCode.Back);
                    break;
                case AndroidKeyCodes.Home:
                    driver.PressKeyCode(AndroidKeyCode.Home);
                    break;
            }
        }
    }
}
