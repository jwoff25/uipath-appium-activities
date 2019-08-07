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

        protected override void Execute(CodeActivityContext context)
        {
            // Inherit driver from scope activity
            var driver = context.DataContext.GetProperties()["Driver"].GetValue(context.DataContext) as AndroidDriver<AndroidElement>;

            switch(KeyCode)
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
