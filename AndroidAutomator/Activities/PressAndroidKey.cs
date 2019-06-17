using System.Activities;
using System.ComponentModel;
using OpenQA.Selenium.Appium.Android;

namespace AndroidAutomator.Activities
{

    public class PressAndroidKey : CodeActivity
    {
        [Category("Type")]
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
