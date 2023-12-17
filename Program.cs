//importing necessary libraries
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

//setting up ChromeDriver and WebDriverWait
IWebDriver driver = new ChromeDriver();
WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

//asking the user to choose a functionality
Console.WriteLine("Hi, welcome to my program \nChoose a functionality (YouTube, Jobs, Lego): ");
//reading the user input
var userSelection = Console.ReadLine();

//handling the YouTube functionality
if (userSelection == "YouTube")
{
    //navigating to the YouTube website
    driver.Navigate().GoToUrl("https://www.youtube.com/");

    //accepting the cookies popup
    var cookies = driver.FindElement(By.XPath("//*[@id=\"content\"]/div[2]/div[6]/div[1]/ytd-button-renderer[1]/" +
        "yt-button-shape/button/yt-touch-feedback-shape/div/div[2]"));
    cookies.Click();

    //finding the search box element by its name attribute
    IWebElement searchBox = driver.FindElement(By.Name("search_query"));
    //clearing any existing text in the search box
    searchBox.Clear();
    //asking the user to enter a search term
    Console.Write("Enter a search term: ");
    //reading the user input from the console and sending it to the search box
    searchBox.SendKeys(Console.ReadLine());
    //submitting the form by calling the Submit method on the search input box
    searchBox.Submit();

    //finding and clicking the filter button on the search results page
    IWebElement filter = driver.FindElement(By.XPath("/html/body/ytd-app/div[1]/ytd-page-manager/ytd-search/div[1]/div/ytd-search-header-renderer/div[3]/ytd-button-renderer/yt-button-shape/button/yt-touch-feedback-shape/div/div[2]"));
    filter.Click();

    //finding and clicking the "Uploaddate" filter option in the dialog
    IWebElement last_upload = driver.FindElement(By.XPath("/html/body/ytd-app/ytd-popup-container/" +
        "tp-yt-paper-dialog/ytd-search-filter-options-dialog-renderer/div[2]/ytd-search-filter-group-renderer[5]/" +
        "ytd-search-filter-renderer[2]/a/div/yt-formatted-string"));
    last_upload.Click();

    //finding all the video elements on the search results page
    ReadOnlyCollection<IWebElement> videos = driver.FindElements(By.CssSelector("#contents > ytd-video-renderer"));
    //creating a list to store information about each video
    List<Dictionary<string, string>> videoList = new List<Dictionary<string, string>>();

    //using StreamWriter to write information about YouTube videos to a CSV file
    using (StreamWriter writer = new StreamWriter("youtube_videos.csv"))
    {
        //initializing a counter for the loop
        var count = 0;
        //processing information for 5 videos
        while (count < 5)
        {
            //introducing a delay to allow the page to load
            Thread.Sleep(250);
            //finding elements for title, links, uploader, and views for the current video
            var titles = videos[count].FindElement(By.CssSelector("#video-title > yt-formatted-string"));
            var links = videos[count].FindElement(By.CssSelector("#video-title"));
            var uploaders = videos[count].FindElement(By.XPath("div[1]/div/div[2]/ytd-channel-name/div/div/yt-formatted-string/a"));
            var views = videos[count].FindElement(By.CssSelector("#metadata-line > span:nth-child(3)"));

            //extracting text content from the found elements
            string title = titles.Text;
            string uploader = uploaders.Text;
            string view = views.Text;

            //printing the information to the console
            Console.WriteLine("Title: " + title);
            Console.WriteLine("Uploader: " + uploader);
            Console.WriteLine("Views: " + view);
            Console.WriteLine("Link: " + links.GetAttribute("href"));
            Console.WriteLine("\n");

            //writing information to the CSV file
            writer.WriteLine($"Title: {title}\nUploader: {uploader}\nVieuws: {view}\nLink: {links.GetAttribute("href")}\n-------");

            //creating a dictionary to store the video information
            var videoData = new Dictionary<string, string>
                {
                    { "Title", title },
                    { "Uploader", uploader },
                    { "Views", view },
                    { "Link", links.GetAttribute("href") }
                };

            //adding the video information to the list
            videoList.Add(videoData);
            //increasing the counter
            count++;
        }
    }
    //serializing the videoList to JSON
    string json = JsonConvert.SerializeObject(videoList, Formatting.Indented);
    //writing the JSON to a file named "youtube_videos.json"
    File.WriteAllText("youtube_videos.json", json);

    //reading a line from the console (prevents the console window from closing immediately)
    Console.ReadLine();
    //quitting the WebDriver
    driver.Quit();
}


//handling the "Jobs" functionality
else if (userSelection == "Jobs")
{
    //navigating to the ICTJob website
    driver.Navigate().GoToUrl("https://www.ictjob.be/");

    //finding the search box element by its name attribute
    IWebElement searchBox = driver.FindElement(By.CssSelector("#keywords-input"));
    //clearing any existing text in the search box
    searchBox.Clear();
    //asking the user to enter a search term
    Console.Write("Enter a search term: ");
    //reading the user input from the console and sending it to the search box
    searchBox.SendKeys(Console.ReadLine());
    //introducing a delay to allow the page to load before interacting with elements
    Thread.Sleep(1000);

    //finding and clicking the "Search" button
    IWebElement get_vacatures = driver.FindElement(By.CssSelector("#main-search-button"));
    get_vacatures.Click();
    //introducing a delay to allow the page to load before interacting with elements
    Thread.Sleep(4000);

    //handling cookies popup
    var cookies = driver.FindElement(By.XPath("//*[@id=\"cookie-info-text\"]/div/div/div[2]"));
    cookies.Click();
    //introducing a delay to allow the page to load before interacting with elements
    Thread.Sleep(1000);

    //finding and clicking the "Sort by Date" option
    IWebElement date = driver.FindElement(By.CssSelector("#sort-by-date"));
    date.Click();
    //introducing a delay to allow the page to load before interacting with elements
    Thread.Sleep(15000);

    //finding all the job data elements on the search results page
    ReadOnlyCollection<IWebElement> data = driver.FindElements(By.CssSelector("#search-result-body > div > ul > li > span.job-info"));
    //creating a list to store information about each job listing
    List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();

    //using StreamWriter to write information about job listings to a CSV file
    using (StreamWriter writer = new StreamWriter("jobs_data.csv"))
    {
        //initializing a counter for the loop
        var count = 0;
        //processing information for 5 job listings
        while (count < 5)
        {
            //introducing a small delay to allow the page to load
            Thread.Sleep(250);
            //finding elements for title, link, location, and company for the current job listing
            var titles = data[count].FindElement(By.CssSelector("a > h2"));
            var link = data[count].FindElement(By.CssSelector("a"));
            var locations = data[count].FindElement(By.CssSelector("span.job-location"));
            var companys = data[count].FindElement(By.CssSelector("span.job-company"));

            //extracting text content from the found elements
            string title = titles.Text;
            string location = locations.Text;
            string company = companys.Text;

            //printing the information to the console
            Console.WriteLine("Title: " + title);
            Console.WriteLine("Location: " + location);
            Console.WriteLine("Company: " + company);
            Console.WriteLine("Link: " + link.GetAttribute("href"));
            
            //trying to find and print keywords
            try
            {
                var keywords = data[count].FindElement(By.CssSelector("span.job-keywords"));
                string keyword = keywords.Text;
                Console.WriteLine("Keywords: " + keyword);
            }
            //handling the case when no keywords are available
            catch (NoSuchElementException)
            {
                Console.WriteLine("No keywords available");
            }
            //printing a new line for better readability in the console
            Console.WriteLine("\n");
            //writing information to the CSV file
            writer.WriteLine($"Title: {title}\nLocation: {location}\nCompany: {company}\nLink: {link.GetAttribute("href")}\n");

            //creating a dictionary to store job information
            var jobsData = new Dictionary<string, string>
                {
                    { "Title", title },
                    { "Location", location },
                    { "Company", company },
                    { "Link", link.GetAttribute("href") }
                };

            //adding the job information to the list
            dataList.Add(jobsData);
            //increasing the counter
            count++;
        }
    }
    //serializing the videoList to JSON
    string json = JsonConvert.SerializeObject(dataList, Formatting.Indented);
    //writing the JSON to a file named "jobs_data.json"
    File.WriteAllText("jobs_data.json", json);

    //reading a line from the console (prevents the console window from closing immediately)
    Console.ReadLine();
    //quitting the WebDriver
    driver.Quit();
}

//handling the "Lego" functionality
else if (userSelection == "Lego")
{
    //prompting the user to enter a year
    Console.Write("Enter a year: ");
    //reads the user's input and assigns it to the variable 
    string year = Console.ReadLine();

    //navigating to the Brickset website with the specified year
    driver.Navigate().GoToUrl($"https://brickset.com/sets/year-{year}/filter-HasImage");

    //finding all the Lego set elements on the page
    ReadOnlyCollection<IWebElement> sets = driver.FindElements(By.CssSelector("div.outerwrap > div > div > section article"));
    //creating a list to store information about each Lego set
    List<Dictionary<string, string>> legoList = new List<Dictionary<string, string>>();

    //using StreamWriter to write information about Lego sets to a CSV file
    using (StreamWriter writer = new StreamWriter("lego_sets.csv"))
    {
        //initializing a counter for the loop
        var count = 0;
        //processing information for 10 lego sets
        while (count < 10)
        {
            //introducing a delay to allow the page to load
            Thread.Sleep(1000);
            //finding elements for title, number, pieces, tags, and link for the current Lego set
            var titleElement = sets[count].FindElement(By.CssSelector("div.meta > h1 > a"));
            var spanElement = titleElement.FindElement(By.CssSelector("span"));
            var numberElement = sets[count].FindElement(By.CssSelector("div.meta > h1 > a > span"));
            var pieces = sets[count].FindElement(By.CssSelector("div.meta > div:nth-child(5) > dl :nth-child(2)"));
            var tags = sets[count].FindElement(By.CssSelector("div.meta > div:nth-child(2)"));
            var link = sets[count].FindElement(By.CssSelector("div.meta > h1 > a"));

            //extracting and processing information
            string title = titleElement.Text;
            string titleWithoutNumber = spanElement != null ? title.Replace(spanElement.Text, "") : title; //(removes text of spanElement)
            string titles = titleWithoutNumber.Trim();
            string number = numberElement.Text.Trim(':');
            string tag = tags.Text;

            //printing the extracted information to the console
            Console.WriteLine("Title: " + titles);
            Console.WriteLine("Number: " + number);
            Console.WriteLine("Pieces: " + pieces.Text);
            Console.WriteLine("Tags: " + tag);
            Console.WriteLine("Link: " + link.GetAttribute("href"));

            // Writing information to the CSV file
            writer.WriteLine($"Title: {titles}\nNumber: {number}\nPieces: {pieces}\nTags: {tag}");
            //creating a dictionary to store Lego set information
            var legoData = new Dictionary<string, string>
            {
                { "Title", titles },
                { "Number", number },
                { "Pieces", pieces.Text },
                { "Tags", tag },
                { "Link", link.GetAttribute("href") },
            };
            //trying to find and print the price
            try
            {
                //trying to find the price element for the current Lego set
                var priceElement = sets[count].FindElement(By.XPath(".//a[@class='plain' and contains(@title, 'Link to external website')]"));
                //extracting the text content of the price element
                string priceAsString = priceElement.Text;
                //cleaning the extracted text to keep only letters, digits, and the dot character
                string cleanedPrice = new string(priceAsString.Where(c => char.IsLetterOrDigit(c) || c == '.').ToArray());
                //printing the cleaned price to the console
                Console.WriteLine("Price: $" + cleanedPrice);
                //writing the cleaned price to the CSV file
                writer.WriteLine($"Price: ${cleanedPrice}");
                //storing the cleaned price in the legoData dictionary
                legoData["Price"] = $"${cleanedPrice}";
            }
            catch (NoSuchElementException)
            {
                //handling the case when no price is available
                Console.WriteLine("No price available");
                //storing a default value in the csv for the price
                writer.WriteLine("No price available");
                //storing a default value in the legoData dictionary for the price
                legoData["Price"] = "No price available";

            }
            //writing the link to the CSV file
            writer.WriteLine($"Link: {link.GetAttribute("href")}\n");
            //adding the Lego set information to the list
            legoList.Add(legoData);
            // Printing a new line for better readability in the console
            Console.WriteLine("\n");
            //increasing the counter
            count++;
        }
    }
    //serializing the videoList to JSON
    string json = JsonConvert.SerializeObject(legoList, Formatting.Indented);
    //writing the JSON to a file named "lego_sets.json"
    File.WriteAllText("lego_sets.json", json);

    //reading a line from the console (prevents the console window from closing immediately)
    Console.ReadLine();
    //quitting the WebDriver
    driver.Quit();
}


