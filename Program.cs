using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;


IWebDriver driver = new ChromeDriver();
WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
Console.WriteLine("Hi, welcome to my program \nChoose a functionality (YouTube, Jobs, Lego): ");
var userSelection = Console.ReadLine();
if (userSelection == "YouTube")
{
    driver.Navigate().GoToUrl("https://www.youtube.com/");

    var cookies = driver.FindElement(By.XPath("//*[@id=\"content\"]/div[2]/div[6]/div[1]/ytd-button-renderer[1]/yt-button-shape/button/yt-touch-feedback-shape/div/div[2]"));
    cookies.Click();

    IWebElement searchBox = driver.FindElement(By.Name("search_query"));
    searchBox.Clear();
    Console.Write("Enter a search term: ");
    searchBox.SendKeys(Console.ReadLine());
    searchBox.Submit();

    IWebElement filter = driver.FindElement(By.XPath("/html/body/ytd-app/div[1]/ytd-page-manager/ytd-search/div[1]/div/ytd-search-header-renderer/div[3]/ytd-button-renderer/yt-button-shape/button/yt-touch-feedback-shape/div/div[2]"));
    filter.Click();

    IWebElement last_upload = driver.FindElement(By.XPath("/html/body/ytd-app/ytd-popup-container/tp-yt-paper-dialog/ytd-search-filter-options-dialog-renderer/div[2]/ytd-search-filter-group-renderer[5]/ytd-search-filter-renderer[2]/a/div/yt-formatted-string"));
    last_upload.Click();

    ReadOnlyCollection<IWebElement> videos = driver.FindElements(By.CssSelector("#contents > ytd-video-renderer"));
    List<Dictionary<string, string>> videoList = new List<Dictionary<string, string>>();

    using (StreamWriter writer = new StreamWriter("youtube_videos.csv"))
    {
        var count = 0;
        while (count < 5)
        {
            Thread.Sleep(250);
            var titles = videos[count].FindElement(By.CssSelector("#video-title > yt-formatted-string"));
            var links = videos[count].FindElement(By.CssSelector("#video-title"));
            var uploaders = videos[count].FindElement(By.XPath("div[1]/div/div[2]/ytd-channel-name/div/div/yt-formatted-string/a"));
            var views = videos[count].FindElement(By.CssSelector("#metadata-line > span:nth-child(3)"));
            string title = titles.Text;
            string uploader = uploaders.Text;
            string view = views.Text;
            Console.WriteLine("Title: " + title);
            Console.WriteLine("Uploader: " + uploader);
            Console.WriteLine("Views: " + view);
            Console.WriteLine("Link: " + links.GetAttribute("href"));
            Console.WriteLine("\n");
            writer.WriteLine($"Title: {title}\nUploader: {uploader}\nVieuws: {view}\nLink: {links.GetAttribute("href")}\n-------");
            var videoData = new Dictionary<string, string>
                {
                    { "Title", title },
                    { "Uploader", uploader },
                    { "Views", view },
                    { "Link", links.GetAttribute("href") }
                };

            videoList.Add(videoData);
            count++;
        }
    }
    string json = JsonConvert.SerializeObject(videoList, Formatting.Indented);
    File.WriteAllText("youtube_videos.json", json);

    Console.ReadLine();
    driver.Quit();
}


else if (userSelection == "Jobs")
{
    driver.Navigate().GoToUrl("https://www.ictjob.be/");

    IWebElement searchBox = driver.FindElement(By.CssSelector("#keywords-input"));
    searchBox.Clear();
    Console.Write("Enter a search term: ");
    searchBox.SendKeys(Console.ReadLine());

    Thread.Sleep(1000);
    IWebElement get_vacatures = driver.FindElement(By.CssSelector("#main-search-button"));
    get_vacatures.Click();

    Thread.Sleep(4000);
    var cookies = driver.FindElement(By.XPath("//*[@id=\"cookie-info-text\"]/div/div/div[2]"));
    cookies.Click();

    Thread.Sleep(1000);
    IWebElement date = driver.FindElement(By.CssSelector("#sort-by-date"));
    date.Click();

    Thread.Sleep(15000);
    ReadOnlyCollection<IWebElement> data = driver.FindElements(By.CssSelector("#search-result-body > div > ul > li > span.job-info"));
    List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();

    using (StreamWriter writer = new StreamWriter("jobs_data.csv"))
    {
        var count = 0;
        while (count < 5)
        {
            Thread.Sleep(250);
            var titles = data[count].FindElement(By.CssSelector("a > h2"));
            var link = data[count].FindElement(By.CssSelector("a"));
            var locations = data[count].FindElement(By.CssSelector("span.job-location"));
            var companys = data[count].FindElement(By.CssSelector("span.job-company"));
            string title = titles.Text;
            string location = locations.Text;
            string company = companys.Text;
            Console.WriteLine("Title: " + title);
            Console.WriteLine("Location: " + location);
            Console.WriteLine("Company: " + company);
            Console.WriteLine("Link: " + link.GetAttribute("href"));
            try
            {
                var keywords = data[count].FindElement(By.CssSelector("span.job-keywords"));
                string keyword = keywords.Text;
                Console.WriteLine("Keywords: " + keyword);
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("No keywords available");
            }
            Console.WriteLine("\n");
            writer.WriteLine($"Title: {title}\nLocation: {location}\nCompany: {company}\nLink: {link.GetAttribute("href")}\n");
            var jobsData = new Dictionary<string, string>
                {
                    { "Title", title },
                    { "Location", location },
                    { "Company", company },
                    { "Link", link.GetAttribute("href") }
                };

            dataList.Add(jobsData);
            count++;
        }
    }
    string json = JsonConvert.SerializeObject(dataList, Formatting.Indented);
    File.WriteAllText("jobs_data.json", json);
    Console.ReadLine();
    driver.Quit();
}

else if (userSelection == "Lego")
{
https://brickset.com/sets/year-2004/filter-HasImage
    Console.Write("Enter a year: ");
    string year = Console.ReadLine();
    driver.Navigate().GoToUrl($"https://brickset.com/sets/year-{year}/filter-HasImage");

    ReadOnlyCollection<IWebElement> sets = driver.FindElements(By.CssSelector("div.outerwrap > div > div > section article"));
    List<Dictionary<string, string>> legoList = new List<Dictionary<string, string>>();

    using (StreamWriter writer = new StreamWriter("lego_sets.csv"))
    {
        var count = 0;
        while (count < 10)
        {
            Thread.Sleep(1000);
            Console.WriteLine("---------------------------");
            var titleElement = sets[count].FindElement(By.CssSelector("div.meta > h1 > a"));
            string title = titleElement.Text;
            var spanElement = titleElement.FindElement(By.CssSelector("span"));
            string titleWithoutNumber = spanElement != null ? title.Replace(spanElement.Text, "") : title;
            string titles = titleWithoutNumber.Trim();
            Console.WriteLine("Title: " + titles);
            var numberElement = sets[count].FindElement(By.CssSelector("div.meta > h1 > a > span"));
            string number = numberElement.Text.Trim(':');
            Console.WriteLine("Number: " + number);
            var pieces = sets[count].FindElement(By.CssSelector("div.meta > div:nth-child(5) > dl :nth-child(2)"));
            Console.WriteLine("Pieces: " + pieces.Text);
            var tags = sets[count].FindElement(By.CssSelector("div.meta > div:nth-child(2)"));
            string tag = tags.Text;
            Console.WriteLine("Tags: " + tag);
            var link = sets[count].FindElement(By.CssSelector("div.meta > h1 > a"));
            Console.WriteLine("Link: " + link.GetAttribute("href"));
            writer.WriteLine($"Title: {titles}\nNumber: {number}\nPieces: {pieces}\nTags: {tag}");
            var legoData = new Dictionary<string, string>
            {
                { "Title", titles },
                { "Number", number },
                { "Pieces", pieces.Text },
                { "Tags", tag },
                { "Link", link.GetAttribute("href") },
            };
            try
            {
                var priceElement = sets[count].FindElement(By.XPath(".//a[@class='plain' and contains(@title, 'Link to external website')]"));
                string priceAsString = priceElement.Text;
                string cleanedPrice = new string(priceAsString.Where(c => char.IsLetterOrDigit(c) || c == '.').ToArray());
                Console.WriteLine("Price: $" + cleanedPrice);
                writer.WriteLine($"Price: ${cleanedPrice}");
                legoData["Price"] = $"${cleanedPrice}";
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("No price available");
                writer.WriteLine("No price available");
                legoData["Price"] = "No price available";

            }
            writer.WriteLine($"Link: {link.GetAttribute("href")}\n");
            legoList.Add(legoData);
            count++;
        }
    }
    string json = JsonConvert.SerializeObject(legoList, Formatting.Indented);
    File.WriteAllText("lego_sets.json", json);

    Console.ReadLine();
    driver.Quit();
}


