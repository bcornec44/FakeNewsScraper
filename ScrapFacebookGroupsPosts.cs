using OpenQA.Selenium;
using System.Diagnostics;
namespace FakeNewsScraper
{
    internal class ScrapFacebookGroupsPosts : BaseScrap
    {
        public ScrapFacebookGroupsPosts(Dal dal, IWebDriver driver) : base(dal, driver)
        {
        }

        public void SaveFacebookGroupsPosts(string groupLink)
        {
            driver.Navigate().GoToUrl(groupLink);
            Wait(3000);
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stopwatch.Elapsed < TimeSpan.FromSeconds(20))
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                Wait(500);
            }

            var feed = FindByClasses(driver, "x9f619 x1n2onr6 x1ja2u2z xeuugli xs83m0k x1xmf6yo x1emribx x1e56ztr x1i64zmx xjl7jj x19h7ccj xu9j1y6 x7ep2pv");
            var groups = FindManyByClasses(feed, "x1yztbdb x1n2onr6 xh8yej3 x1ja2u2z");
            var i = 0;
            foreach (var group in groups)
            {
                i++;
                try
                {
                    Focus(group);
                    //var groupContent = group.Text;
                    //foreach (var keyword in FakeNewsKeywords)
                    //{
                    //    if (groupContent.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    //    {
                    SaveGroupPostToDatabase(group, groupLink);
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



        private void SaveGroupPostToDatabase(IWebElement webElement, string groupLink)
        {
            var facebookGroup = new FacebookGroupPost
            {
                GroupLink = groupLink,
                Summary = webElement.Text,
                Text = ReadPostText(webElement),
                Link = ReadPostLink(webElement),
            };
            dal.Save(facebookGroup);
        }

        private string ReadPostText(IWebElement webElement)
        {
            Focus(webElement);
            var postText = FindByClasses(webElement, "x193iq5w xeuugli x13faqbe x1vvkbs x1xmvt09 x1lliihq x1s928wv xhkezso x1gmr53x x1cpjm7i x1fgarty x1943h6x xudqn12 x3x7a5m x6prxxf xvq8zen xo1l8bm xzsf02u x1yc453h");
            Focus(postText);
            string content = postText.Text; 
            return content;
        }

        private string ReadPostLink(IWebElement webElement)
        {
            Focus(webElement);
            var postLink = FindByClasses(webElement, "x1i10hfl xjbqb8w x6umtig x1b1mbwd xaqea5y xav7gou x9f619 x1ypdohk xt0psk2 xe8uvvx xdj266r x11i5rnm xat24cr x1mh8g0r xexx8yu x4uap5 x18d9i69 xkhd6sd x16tdsg8 x1hl2dhg xggy1nq x1a2a7pz x1heor9g xt0b8zv xo1l8bm");
            Focus(postLink);
            var content = postLink.GetAttribute("href");
            return content;
        }

    }
}
