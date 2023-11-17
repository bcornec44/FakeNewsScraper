using Dapper;
using Dapper.FastCrud;
using Microsoft.Data.Sqlite;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FakeNewsScraper
{
    internal class BaseScrap
    {
        protected readonly Dal dal;
        protected readonly IWebDriver driver;
        protected readonly IJavaScriptExecutor js;

        public BaseScrap(Dal dal, IWebDriver driver)
        {
            this.dal = dal;
            this.driver = driver;
            this.js = (IJavaScriptExecutor)driver;
        }

        protected void Wait(int timeout)
        {
            Task.Delay(timeout).Wait();
        }

        protected void Focus(IWebElement webElement)
        {
            Wait(50);
            new Actions(driver).MoveToElement(webElement).Perform();
            Wait(100);
        }

        protected IWebElement FindByClasses(ISearchContext searchContext, string cssClasses)
        {
            string xPath = GetXPath(cssClasses);
            IWebElement element = searchContext.FindElement(By.XPath(xPath));
            return element;
        }

        protected IList<IWebElement> FindManyByClasses(ISearchContext searchContext, string cssClasses)
        {
            string xPath = GetXPath(cssClasses);
            var element = searchContext.FindElements(By.XPath(xPath));
            return element;
        }
        private static string GetXPath(string cssClasses)
        {
            var classes = cssClasses.Split(" ");
            var xPath = $".//*[contains(@class, '{classes.First()}')";
            foreach (var c in classes.Skip(1))
            {
                xPath += $" and contains(@class, '{c}')";
            }
            xPath += "]";
            return xPath;
        }

        protected static string RegRead(string input, string pattern)
        {
            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return input;
        }

        protected static void LogError(string message)
        {
            Console.WriteLine(message);
        }
    }
}
