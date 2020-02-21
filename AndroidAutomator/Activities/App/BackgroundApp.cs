using System.Activities;
using AndroidAutomator.Properties;
using OpenQA.Selenium.Appium.Android;

// Not working -- Fix later
namespace AndroidAutomator.Activities.App
{
    [LocalizedDisplayName(nameof(Resources.BackgroundAppActivityName))]
    [LocalizedDescription(nameof(Resources.BackgroundAppActivityDesc))]
    class BackgroundApp : CodeActivity
    {
        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.BackgroundTimeoutField))]
        [LocalizedDescription(nameof(Resources.BackgroundTimeoutDesc))]
        [RequiredArgument]
        public InArgument<int> Timeout { get; set; } = -1;

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

            int timeout = Timeout.Get(context);

            driver.BackgroundApp(timeout);
        }

    }
}
