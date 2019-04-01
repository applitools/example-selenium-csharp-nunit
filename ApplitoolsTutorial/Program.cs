using Applitools;
using Applitools.Selenium;
using Applitools.VisualGrid;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Configuration = Applitools.Selenium.Configuration;


namespace ApplitoolsTutorial
{

    class Program
    {
        public static void Main()
        {
            Program program = new Program();
            program.Run();
        }

        private void Run()
        {
            // Create a new webdriver
            IWebDriver webDriver = new ChromeDriver();

            // Navigate to the url we want to test
            webDriver.Url = "https:/demo.applitools.com";

            // ⭐️ Note to see visual bugs, run the test using the above URL for the 1st run.
            //but then change the above URL to https://demo.applitools.com/index_v2.html (for the 2nd run)

            // Create a runner with concurrency of 10
            VisualGridRunner runner = new VisualGridRunner(10);

            // Create Eyes object with the runner, meaning it'll be a Visual Grid eyes.
            Eyes eyes = new Eyes(runner);

            //Set the Applitools API KEY here or as an environment variable "APPLITOOLS_API_KEY"
            eyes.ApiKey = "APPLITOOLS_API_KEY";


            // Create configuration object
            Configuration conf = new Configuration();
            

            // Set test name
            conf.TestName = "C# VisualGrid demo";

            // Set app name
            conf.AppName = "Demo app";

            // Add browsers with different viewports
            conf.AddBrowser(800, 600, Configuration.BrowserType.CHROME);
            conf.AddBrowser(700, 500, Configuration.BrowserType.CHROME);
            conf.AddBrowser(1200, 800, Configuration.BrowserType.FIREFOX);
            conf.AddBrowser(1600, 1200, Configuration.BrowserType.FIREFOX);

            // Add iPhone 4 device emulation in Portraig mode
            EmulationInfo iphone4 = new EmulationInfo(EmulationInfo.DeviceNameEnum.iPhone_4, Applitools.VisualGrid.ScreenOrientation.Portrait);
            conf.AddDeviceEmulation(iphone4);



            // Set the configuration object to eyes
            eyes.Configuration = conf;

            // Call Open on eyes to initialize a test session
            eyes.Open(webDriver);

            // check the login page
            eyes.Check(Target.Window().Fully().WithName("Login page"));
            webDriver.FindElement(By.Id("log-in")).Click();

            // Check the app page
            eyes.Check(Target.Window().Fully().WithName("App page"));

            // Close the browser
            webDriver.Quit();

            //Wait and collect all test results
            TestResultSummary allTestResults = runner.GetAllTestResults();
            System.Console.WriteLine(allTestResults);
        }

    }
}
