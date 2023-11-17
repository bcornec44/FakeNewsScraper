using Microsoft.Data.Sqlite;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeNewsScraper
{
    internal class ScrapFacebookGroups : BaseScrap
    {
        public ScrapFacebookGroups(Dal dal, IWebDriver driver) : base(dal, driver)
        {
        }

        public void SaveFacebookGroups(IWebDriver driver, string groupKeyWord)
        {
            var groupsUrl = $"https://www.facebook.com/groups/search/groups_home/?q={groupKeyWord}";
            driver.Navigate().GoToUrl(groupsUrl);
            Wait(3000);
            driver.Navigate().GoToUrl($"{groupsUrl}&__epa__=SERP_TAB&__eps__=GROUPS_HOME_SERP_ALL_TAB");
            Wait(300);
            var publicGroupButton = driver.FindElement(By.XPath("//input[@aria-label='Groupes publics']"));
            publicGroupButton.Click();
            Wait(300);
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stopwatch.Elapsed < TimeSpan.FromSeconds(60))
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                Wait(500);
            }

            var groups = FindManyByClasses(driver, "x1yztbdb");

            var i = 0;
            foreach (var group in groups)
            {
                i++;
                try
                {
                    var groupContent = group.Text;
                    //foreach (var keyword in FakeNewsKeywords)
                    //{
                    //    if (groupContent.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    //    {
                    SaveGroupToDatabase(group);
                    //        break;
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    LogError(i + " - " + ex.Message);
                }
            }
        }

        private void SaveGroupToDatabase(IWebElement webElement)
        {
            string content = webElement.GetAttribute("outerHTML");

            var facebookGroup = new FacebookGroup 
            {
                Content = content,
                Summary = webElement.Text,
                Title = RegReadTitle(content),
                Link = RegReadLink(content),
            };

            dal.Save(facebookGroup);
        }


        private static string RegReadTitle(string input)
        {
            return RegRead(input, "role=\"presentation\" tabindex=\"-1\">(.*?)</a>");
        }

        private static string RegReadLink(string input)
        {
            return RegRead(input, "<a aria-hidden=\"true\" class=\"x1i10hfl xjbqb8w x6umtig x1b1mbwd xaqea5y xav7gou x9f619 x1ypdohk xt0psk2 xe8uvvx xdj266r x11i5rnm xat24cr x1mh8g0r xexx8yu x4uap5 x18d9i69 xkhd6sd x16tdsg8 x1hl2dhg xggy1nq x1a2a7pz xt0b8zv xzsf02u x1s688f\" href=\"(.*?)\" role=\"presentation\" tabindex=\"-1\"");
        }
    }
}
