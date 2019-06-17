using System;
using System.Activities;
using System.Activities.Statements;
using System.ComponentModel;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;

namespace AndroidAutomator.Activities
{
    [Designer(typeof(AndroidEmulatorScopeDesigner))]
    public class AndroidEmulatorScope : NativeActivity
    {
        [Browsable(false)]
        public ActivityAction<AndroidDriver<AndroidElement>> Body { get; set; }

        [Category("App")]
        [RequiredArgument]
        public InArgument<string> ApkPath { get; set; }

        [Category("Emulator")]
        [RequiredArgument]
        public InArgument<string> DeviceName { get; set; }

        [Category("Emulator")]
        [RequiredArgument]
        public InArgument<string> AndroidVersion { get; set; }

        [Category("App")]
        [RequiredArgument]
        public InArgument<string> AppPackage { get; set; }

        [Category("App")]
        [RequiredArgument]
        public InArgument<string> AppActivity { get; set; }

        [Category("Setup")]
        public InArgument<int> WaitTime { get; set; }

        [Category("Setup")]
        public InArgument<string> Locale { get; set; }

        [Category("Setup")]
        public InArgument<string> Language { get; set; }

        public AndroidEmulatorScope()
        {
            Body = new ActivityAction<AndroidDriver<AndroidElement>>
            {
                Argument = new DelegateInArgument<AndroidDriver<AndroidElement>>("Driver"),
                Handler = new Sequence { DisplayName = "Do" }
            };
        }

        protected override void Execute(NativeActivityContext context)
        {
            // Gather fields
            string apkPath = ApkPath.Get(context);
            string deviceName = DeviceName.Get(context);
            string version = AndroidVersion.Get(context);
            string package = AppPackage.Get(context);
            string activity = AppActivity.Get(context);
            string locale = Locale.Get(context) ?? "US";
            string language = Language.Get(context) ?? "en";
            int waitTime = WaitTime.Get(context);


            // Initialize Driver and Appium Server
            AndroidDriver<AndroidElement> driver;
            AppiumLocalService server;

            // Start Appium Server on any open port
            server = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            server.Start();

            // Set additional capabilities
            var options = new AppiumOptions();
            //Device capabilities 
            options.AddAdditionalCapability(MobileCapabilityType.DeviceName, deviceName);
            options.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            options.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, version);
            options.AddAdditionalCapability(MobileCapabilityType.Locale, locale);
            options.AddAdditionalCapability(MobileCapabilityType.Language, language);
            //App capabilities
            options.AddAdditionalCapability(MobileCapabilityType.App, apkPath);
            options.AddAdditionalCapability(AndroidMobileCapabilityType.AppPackage, package);
            options.AddAdditionalCapability(AndroidMobileCapabilityType.AppActivity, activity);

            driver = new AndroidDriver<AndroidElement>(server, options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(waitTime);

            // Schedule Activities
            if (Body != null)
            {
                context.ScheduleAction<AndroidDriver<AndroidElement>>(Body, driver, OnCompleted, OnFaulted);
            }
        }

        private void OnFaulted(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
        {
            //TODO
        }

        private void OnCompleted(NativeActivityContext context, ActivityInstance completedInstance)
        {
            //TODO
        }
    }
}
