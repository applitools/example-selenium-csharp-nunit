using Applitools;
using Applitools.Selenium;
using Applitools.VisualGrid;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Configuration = Applitools.Selenium.Configuration;
using ScreenOrientation = Applitools.VisualGrid.ScreenOrientation;

namespace ApplitoolsTutorial
{

    [TestFixture]
    public class VisualGridDemo
    {

        IWebDriver driver;
        VisualGridRunner runner;
        Eyes eyes;

        [SetUp]
        public void BeforeEach()
        {
            // Use Chrome browser
            driver = new ChromeDriver();

            //Create a runner with concurrency of 10
            runner = new VisualGridRunner(10);

            // Initialize the eyes SDK (IMPORTANT: make sure your API key is set in the APPLITOOLS_API_KEY env variable).
            eyes = new Eyes(runner);

            // Get current Eyes configuration object
            Configuration conf = eyes.GetConfiguration();


            conf.SetApiKey("APPLITOOLS_API_KEY");  // Set the Applitools API KEY here or as an environment variable "APPLITOOLS_API_KEY"

            // Add browsers with different viewports
            conf.AddBrowser(800, 600, BrowserType.CHROME);
            conf.AddBrowser(700, 500, BrowserType.FIREFOX);
            conf.AddBrowser(1600, 1200, BrowserType.IE_11);
            conf.AddBrowser(1024, 768, BrowserType.EDGE_CHROMIUM);
            conf.AddBrowser(800, 600, BrowserType.SAFARI);

            // Add iPhone 4 device emulation in Portrait mode
            conf.AddDeviceEmulation(DeviceName.iPhone_X, ScreenOrientation.Portrait);
            conf.AddDeviceEmulation(DeviceName.Pixel_2, ScreenOrientation.Portrait);


            // Set the configuration object to eyes
            eyes.SetConfiguration(conf);
        }

        [Test]
        public void VGTest()
        {
            // Navigate to the url we want to test
            driver.Url = "https://demo.applitools.com";
            // ⭐️ Note to see visual bugs, run the test using the above URL for the 1st run.
            //but then change the above URL to https://demo.applitools.com/index_v2.html (for the 2nd run)

            // Call Open on eyes to initialize a test session
            eyes.Open(driver, "Demo App", "Ultrafast grid demo");

            // check the login page
            eyes.Check(Target.Window().Fully().WithName("Login page"));
            driver.FindElement(By.Id("log-in")).Click();

            // Check the app page
            eyes.Check(Target.Window().Fully().WithName("App page"));

            // Call Close on eyes to let the server know it should display the results
            eyes.CloseAsync();
        }

        [TearDown]
        public void AfterEach()
        {
            // Close the browser
            driver.Quit();
            // Wait and collect all test results
            // we pass false to this method to suppress the exception that is thrown if we
            // find visual differences
            TestResultsSummary allTestResults = runner.GetAllTestResults(false);
            // Print results
            System.Console.WriteLine(allTestResults);
            // If the test was aborted before eyes.close was called, ends the test as aborted.
            eyes.AbortIfNotClosed();
        }

    }
}
