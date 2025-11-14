using Application.Interfaces.Downloaders;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Downloaders
{
    internal class AmazonHtmlDownloader : IHtmlDownloader
    {
        private ChromeOptions _chromeOptions;

        public AmazonHtmlDownloader()
        {
            _chromeOptions = new ChromeOptions();

            _chromeOptions.AddArgument("--headless");
            
            //for driver to work with amazon in "--headless" mode
            _chromeOptions.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
        }

        public async Task<string> DownloadHtmlAsync(string url)
        {
            return await Task.Run(() => DownloadHtml(url));
        }
        private string DownloadHtml(string url)
        {
            using var driver = new ChromeDriver(_chromeOptions);

            driver.Url = url;


            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d =>
            {
                try
                {
                    bool loaded = false;

                    IWebElement playlist = driver.FindElement(By.CssSelector("music-detail-header[secondary-text]"));

                    IWebElement track = driver.FindElement(By.CssSelector("music-image-row[primary-text]"));

                    loaded = playlist.Displayed && track.Displayed;

                    return loaded;
                }
                catch
                {
                    return false;
                }
            });

            string html = driver.PageSource;

            driver.Quit();

            return html;
        }
    }
}
