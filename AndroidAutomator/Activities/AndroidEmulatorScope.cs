using System;
using AndroidAutomator.Properties;
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
    [LocalizedDisplayName(nameof(Resources.ScopeActivityName))]
    [LocalizedDescription(nameof(Resources.ScopeActivityDesc))]
    public class AndroidEmulatorScope : NativeActivity
    {
        [Browsable(false)]
        public ActivityAction<AndroidDriver<AndroidElement>> Body { get; set; }

        [LocalizedCategory(nameof(Resources.Device))]
        [LocalizedDisplayName(nameof(Resources.DeviceNameField))]
        [LocalizedDescription(nameof(Resources.DeviceNameDesc))]
        [RequiredArgument]
        public InArgument<string> DeviceName { get; set; }

        [LocalizedCategory(nameof(Resources.Device))]
        [LocalizedDisplayName(nameof(Resources.AndroidVersionField))]
        [LocalizedDescription(nameof(Resources.AndroidVersionDesc))]
        [RequiredArgument]
        public InArgument<string> AndroidVersion { get; set; }

        [LocalizedCategory(nameof(Resources.App))]
        [LocalizedDisplayName(nameof(Resources.ApkPathField))]
        [LocalizedDescription(nameof(Resources.ApkPathDesc))]
        public InArgument<string> ApkPath { get; set; }

        [LocalizedCategory(nameof(Resources.App))]
        [LocalizedDisplayName(nameof(Resources.AppPackageField))]
        [LocalizedDescription(nameof(Resources.AppPackageDesc))]
        public InArgument<string> AppPackage { get; set; }

        [LocalizedCategory(nameof(Resources.App))]
        [LocalizedDisplayName(nameof(Resources.AppActivityField))]
        [LocalizedDescription(nameof(Resources.AppActivityDesc))]
        public InArgument<string> AppActivity { get; set; }

        [LocalizedCategory(nameof(Resources.Options))]
        [LocalizedDisplayName(nameof(Resources.WaitTimeField))]
        [LocalizedDescription(nameof(Resources.WaitTimeDesc))]
        public InArgument<int> WaitTime { get; set; }

        [LocalizedCategory(nameof(Resources.Options))]
        [LocalizedDisplayName(nameof(Resources.LocaleField))]
        [LocalizedDescription(nameof(Resources.LocaleDesc))]
        public InArgument<string> Locale { get; set; }

        [LocalizedCategory(nameof(Resources.Options))]
        [LocalizedDisplayName(nameof(Resources.LanguageField))]
        [LocalizedDescription(nameof(Resources.LanguageDesc))]
        public InArgument<string> Language { get; set; }

        [LocalizedCategory(nameof(Resources.Options))]
        [LocalizedDisplayName(nameof(Resources.ScreenshotPathField))]
        [LocalizedDescription(nameof(Resources.ScreenshotPathDesc))]
        public InArgument<string> ScreenshotPath { get; set; }

        [LocalizedCategory(nameof(Resources.Options))]
        [LocalizedDisplayName(nameof(Resources.LaunchTypeField))]
        [LocalizedDescription(nameof(Resources.LaunchTypeDesc))]
        [RequiredArgument]
        public LaunchType LaunchType { get; set; }

        [LocalizedCategory(nameof(Resources.Browser))]
        [LocalizedDisplayName(nameof(Resources.ChromedriverPathField))]
        [LocalizedDescription(nameof(Resources.ChromedriverPathDesc))]
        public InArgument<string> ChromedriverPath { get; set; }

        [LocalizedCategory(nameof(Resources.Browser))]
        [LocalizedDisplayName(nameof(Resources.BrowserTypeField))]
        [LocalizedDescription(nameof(Resources.BrowserTypeDesc))]
        public BrowserType BrowserType { get; set; }

        [LocalizedCategory(nameof(Resources.Output))]
        [LocalizedDisplayName(nameof(Resources.DriverField))]
        [LocalizedDescription(nameof(Resources.DriverDesc))]
        public OutArgument<AndroidDriver<AndroidElement>> Driver { get; set; }

        public AndroidEmulatorScope()
        {
            Body = new ActivityAction<AndroidDriver<AndroidElement>>
            {
                Argument = new DelegateInArgument<AndroidDriver<AndroidElement>>("Driver"),
                Handler = new Sequence { DisplayName = "Do" }
            };
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            if (LaunchType == LaunchType.App)
            {
                if (ApkPath == null || AppPackage == null || AppActivity == null)
                {
                    metadata.AddValidationError("If LaunchType is App, then the following fields need to be filled in: ApkPath, AppPackage, AppActivity.");
                }
            }
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
            string screenshotPath = ScreenshotPath.Get(context) ?? "";
            string chromedriverPath = ChromedriverPath.Get(context) ?? "";

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
            // Use UIAutomator2 for better performance
            options.AddAdditionalCapability(MobileCapabilityType.AutomationName, AutomationName.AndroidUIAutomator2);
            switch(LaunchType)
            {
                case LaunchType.App:
                    //App capabilities
                    options.AddAdditionalCapability(MobileCapabilityType.App, apkPath);
                    options.AddAdditionalCapability(AndroidMobileCapabilityType.AppPackage, package);
                    options.AddAdditionalCapability(AndroidMobileCapabilityType.AppActivity, activity);
                    break;
                case LaunchType.Browser:
                    //Browser capabilities
                    switch (BrowserType)
                    {
                        case BrowserType.Chrome:
                            options.AddAdditionalCapability(MobileCapabilityType.BrowserName, MobileBrowserType.Chrome);
                            if (!String.IsNullOrEmpty(chromedriverPath))
                                options.AddAdditionalCapability(AndroidMobileCapabilityType.ChromedriverExecutable, chromedriverPath);
                            break;
                        default:
                            throw new ArgumentNullException("Missing Argument: BrowserType");
                    }
                    break;
                default:
                    throw new ArgumentNullException("Missing Argument: LaunchType");
            }
            //Default Screenshot Path
            options.AddAdditionalCapability(AndroidMobileCapabilityType.AndroidScreenshotPath, screenshotPath);

            driver = new AndroidDriver<AndroidElement>(server, options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(waitTime);

            // Export driver
            Driver.Set(context, driver);

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
            
        }
    }
}
