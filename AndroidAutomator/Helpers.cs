using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Service.Options;

namespace AndroidAutomator
{
    class Helpers
    {
        public static AndroidElement GetElement(AndroidDriver<AndroidElement> driver, SelectBy by, string selector)
        {
            AndroidElement e = null;
            switch (by)
            {
                case SelectBy.Id:
                    e = driver.FindElementById(selector);
                    break;
                case SelectBy.Class:
                    e = driver.FindElementByClassName(selector);
                    break;
                case SelectBy.XPath:
                    e = driver.FindElementByXPath(selector);
                    break;
            }
            return e ?? throw new NotFoundException("Element with selector " + selector + " not found.");
        }

        public static AndroidElement GetElementFromList(AndroidDriver<AndroidElement> driver, SelectBy by, string selector, int index)
        {
            AndroidElement e = null;
            switch (by)
            {
                case SelectBy.Id:
                    e = driver.FindElementsById(selector)[index];
                    break;
                case SelectBy.Class:
                    e = driver.FindElementsByClassName(selector)[index];
                    break;
                case SelectBy.XPath:
                    e = driver.FindElementsByXPath(selector)[index];
                    break;
            }
            return e ?? throw new NotFoundException("Element with selector " + selector + " not found.");
        }
    }
}
