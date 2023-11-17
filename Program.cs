using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Dapper.FastCrud;
using Microsoft.Data.Sqlite;
using OpenQA.Selenium.Support.UI;
using Dapper;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace FakeNewsScraper
{
    class Program
    {
        private static readonly string[] FakeNewsKeywords = { "fake", "hoax", "false" };
        private static readonly string LogFilePath = "app.log";

        static void Main(string[] args)
        {
            try
            {
                var dal = new Dal();

                dal.InitializeDatabase();
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments("--disable-notifications"); // Désactiver les notifications

                using (IWebDriver driver = new ChromeDriver(chromeOptions))
                {
                    ConnectToFacebook(driver);

                    var groupLink = "https://www.facebook.com/groups/384556298256084/";
                    var scrapper = new ScrapFacebookGroupsPosts(dal, driver);
                    scrapper.SaveFacebookGroupsPosts(groupLink);

                    //var groupKeyWord = "eolienne";
                    //SaveFacebookGroups(driver, groupKeyWord);
                }
            }
            catch (Exception ex)
            {
                LogError("Main - " + ex.Message);
            }
        }

        private static void ConnectToFacebook(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://www.facebook.com/");
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var allowCookiesButton = wait.Until(driver => driver.FindElement(By.XPath("//button[text()='Allow all cookies']")));
            allowCookiesButton.Click();
            var usernameField = driver.FindElement(By.Name("email"));
            var passwordField = driver.FindElement(By.Name("pass"));
            usernameField.Clear();
            usernameField.SendKeys("");
            usernameField.SendKeys("siin_corn@hotmail.com");
            Task.Delay(1000).Wait();
            passwordField.Clear();
            passwordField.SendKeys("");
            passwordField.SendKeys("15CA-JMW");
            Task.Delay(1000).Wait();
            var loginField = driver.FindElement(By.Name("login"));
            loginField.Submit();
            Task.Delay(3000).Wait();
        }

        private static void LogError(string message)
        {
            Console.WriteLine(message);
        }
    }
}
