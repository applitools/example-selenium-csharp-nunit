using Applitools;
using Applitools.Selenium;
using Applitools.VisualGrid;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static Applitools.Configuration;
using static Applitools.VisualGrid.ChromeEmulationInfo;
using Configuration = Applitools.Selenium.Configuration;
using ScreenOrientation = Applitools.VisualGrid.ScreenOrientation;

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

            // Create configuration object
            Configuration conf = new Configuration();



            // Set the Applitools API KEY here or as an environment variable "APPLITOOLS_API_KEY"
            conf.SetApiKey("APPLITOOLS_API_KEY")
                .SetTestName("C# VisualGrid demo")   // Set test name
                .SetAppName("Demo app");             // Set app name

            // Add different browsers with different viewports
            conf.AddBrowser(800, 600, BrowserType.CHROME);
            conf.AddBrowser(700, 500, BrowserType.CHROME);
            conf.AddBrowser(1200, 800, BrowserType.FIREFOX);
            conf.AddBrowser(1600, 1200, BrowserType.FIREFOX);

            // Add iPhone 4 device emulation in Portraig mode
            ChromeEmulationInfo iphone4 = new ChromeEmulationInfo(DeviceNameEnum.iPhone_4, ScreenOrientation.Portrait);
            conf.AddDeviceEmulation(iphone4);



            // Set the configuration object to eyes
            eyes.SetConfiguration(conf);

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
