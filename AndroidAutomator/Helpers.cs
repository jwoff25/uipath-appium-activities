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
        // Get element from selector
        public static AndroidElement GetElement(AndroidDriver<AndroidElement> driver, SelectBy by, string selector)
        {
            AndroidElement e = null;
            switch (by)
            {
                case SelectBy.ID:
                    e = driver.FindElementById(selector);
                    break;
                case SelectBy.Class:
                    e = driver.FindElementByClassName(selector);
                    break;
                case SelectBy.XPath:
                    e = driver.FindElementByXPath(selector);
                    break;
                case SelectBy.ExactText:
                    e = driver.FindElementByAndroidUIAutomator(String.Format("new UiSelector().text(\"{0}\")", selector));
                    break;
                case SelectBy.ContainsText:
                    e = driver.FindElementByAndroidUIAutomator(String.Format("new UiSelector().textContains(\"{0}\")", selector));
                    break;
                case SelectBy.RegexText:
                    e = driver.FindElementByAndroidUIAutomator(String.Format("new UiSelector().textMatches(\"{0}\")", selector));
                    break;
            }
            return e ?? throw new NotFoundException("Element with selector " + selector + " not found.");
        }

        // Get a list of elements from selector and return based on index
        public static AndroidElement GetElementFromList(AndroidDriver<AndroidElement> driver, SelectBy by, string selector, int index)
        {
            AndroidElement e = null;
            switch (by)
            {
                case SelectBy.ID:
                    e = driver.FindElementsById(selector)[index];
                    break;
                case SelectBy.Class:
                    e = driver.FindElementsByClassName(selector)[index];
                    break;
                case SelectBy.XPath:
                    e = driver.FindElementsByXPath(selector)[index];
                    break;
                case SelectBy.ExactText:
                    e = driver.FindElementsByAndroidUIAutomator(String.Format("new UiSelector().text(\"{0}\")", selector)).ElementAt(index);
                    break;
                case SelectBy.ContainsText:
                    e = driver.FindElementsByAndroidUIAutomator(String.Format("new UiSelector().textContains(\"{0}\")", selector)).ElementAt(index);
                    break;
                case SelectBy.RegexText:
                    e = driver.FindElementsByAndroidUIAutomator(String.Format("new UiSelector().textMatches(\"{0}\")", selector)).ElementAt(index);
                    break;
            }
            return e ?? throw new NotFoundException("Element with selector " + selector + " not found.");
        }

        // Get child element from parent element based on selector
        public static AndroidElement GetChildElement(AndroidElement parent, SelectBy by, string selector)
        {
            AndroidElement e = null;
            switch (by)
            {
                case SelectBy.ID:
                    e = (AndroidElement)parent.FindElementById(selector);
                    break;
                case SelectBy.Class:
                    e = (AndroidElement)parent.FindElementByClassName(selector);
                    break;
                case SelectBy.XPath:
                    e = (AndroidElement)parent.FindElementByXPath(selector);
                    break;
                case SelectBy.ExactText:
                    e = (AndroidElement)parent.FindElementByAndroidUIAutomator(String.Format("new UiSelector().text(\"{0}\")", selector));
                    break;
                case SelectBy.ContainsText:
                    e = (AndroidElement)parent.FindElementByAndroidUIAutomator(String.Format("new UiSelector().textContains(\"{0}\")", selector));
                    break;
                case SelectBy.RegexText:
                    e = (AndroidElement)parent.FindElementByAndroidUIAutomator(String.Format("new UiSelector().textMatches(\"{0}\")", selector));
                    break;
            }
            return e ?? throw new NotFoundException("Child with selector " + selector + " not found.");
        }

        // Get list of child elements based on selector from parent and return element in index
        public static AndroidElement GetChildElementFromList(AndroidElement parent, SelectBy by, string selector, int index)
        {
            AndroidElement e = null;
            switch (by)
            {
                case SelectBy.ID:
                    e = (AndroidElement)parent.FindElementsById(selector)[index];
                    break;
                case SelectBy.Class:
                    e = (AndroidElement)parent.FindElementsByClassName(selector)[index];
                    break;
                case SelectBy.XPath:
                    e = (AndroidElement)parent.FindElementsByXPath(selector)[index];
                    break;
                case SelectBy.ExactText:
                    e = (AndroidElement)parent.FindElementsByAndroidUIAutomator(String.Format("new UiSelector().text(\"{0}\")", selector)).ElementAt(index);
                    break;
                case SelectBy.ContainsText:
                    e = (AndroidElement)parent.FindElementsByAndroidUIAutomator(String.Format("new UiSelector().textContains(\"{0}\")", selector)).ElementAt(index);
                    break;
                case SelectBy.RegexText:
                    e = (AndroidElement)parent.FindElementsByAndroidUIAutomator(String.Format("new UiSelector().textMatches(\"{0}\")", selector)).ElementAt(index);
                    break;
            }
            return e ?? throw new NotFoundException("Child with selector " + selector + " not found.");
        }
    }
}
