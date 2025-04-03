using SearchScraper.Models;
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace SearchScraper.Services
{
    public class SearchScraperService
    {
        private readonly ISearchRepository _searchRepository;

        public SearchScraperService(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        public List<SearchResult> ScrapeSearchResults(string searchPhrase, string searchUrl)
        {
            List<SearchResult> searchResults = new List<SearchResult>();
            List<string> results = new List<string>();

            string url = $"https://www.google.co.uk/search?num=100&q={Uri.EscapeDataString(searchPhrase)}";

            string htmlContent = GetHtmlUsingSelenium(url);

            if (!string.IsNullOrEmpty(htmlContent))
            {
                List<string> pageResults = ExtractURLResults(htmlContent);
                results.AddRange(pageResults);
            }
            
            searchResults = ParseHtmlForResults(results, searchPhrase, searchUrl);

            foreach (var result in searchResults)
            {
                _searchRepository.SaveSearchResult(result);
            }

            return searchResults;
        }

        private string GetHtmlUsingSelenium(string url)
        {
            var options = new ChromeOptions();
            //options.AddArgument("--headless"); //causes detection
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--log-level=0");

            //Enable JavaScript
            options.AddUserProfilePreference("profile.default_content_setting_values.javascript", 1);

            //Use a fresh Chrome profile to mimic a real user
            options.AddArgument("--profile-directory=Default");

            //Bypass automation detection
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalOption("useAutomationExtension", false);

            //Mimic a real user
            options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36");

            using (IWebDriver driver = new ChromeDriver(options))
            {
                try
                {
                    driver.Navigate().GoToUrl(url);

                    //Check if CAPTCHA is present
                    if (driver.PageSource.Contains("Our systems have detected unusual traffic"))
                    {
                        Console.WriteLine("CAPTCHA detected! Waiting for manual input...");
                        SolveCaptchaManually(driver);
                    }

                    return driver.PageSource;
                }
                catch (WebDriverException ex)
                {
                    Console.WriteLine($"Error fetching page: {ex.Message}");
                    return string.Empty;
                }
            }
        }

        private void SolveCaptchaManually(IWebDriver driver)
        {
            Console.WriteLine("CAPTCHA Detected! Please solve it manually in the browser.");
            Console.WriteLine("Waiting for user input...");

            //Wait for user to solve CAPTCHA manually
            while (driver.PageSource.Contains("Our systems have detected unusual traffic"))
            {
                Thread.Sleep(5000); //Check every 5 seconds
            }

            Console.WriteLine("CAPTCHA solved. Continuing search...");
        }

        public List<string> ExtractURLResults(string html)
        {
            List<string> urls = new List<string>();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            try
            {
                //Use div class 'b8lM7' to find search result nodes
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//div[@class='b8lM7']//a"))
                {
                    string href = link.GetAttributeValue("href", "");

                    if (!string.IsNullOrEmpty(href) && href.StartsWith("http"))
                    {
                        urls.Add(href);
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"Incorrect Search Results: {ex.Message}");
            }

            return urls;
        }

        public List<SearchResult> ParseHtmlForResults(List<string> results, string searchPhrase, string searchUrl)
        {
            List<SearchResult> searchResults = new List<SearchResult>();
            int position = 1;

            foreach (string foundUrl in results)
            {
                if (foundUrl.Contains(searchUrl))
                {
                    searchResults.Add(new SearchResult
                    {
                        SearchPhrase = searchPhrase,
                        SearchUrl = foundUrl,
                        Position = position
                    });
                }
                position++;
            }

            return searchResults;
        }
    }

}
