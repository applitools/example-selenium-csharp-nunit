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

        [Test]
        public void VGTest()
        {
            // Create a new webdriver
            IWebDriver webDriver = new ChromeDriver();

            // Navigate to the url we want to test
            webDriver.Url = "https://demo.applitools.com";

            // ⭐️ Note to see visual bugs, run the test using the above URL for the 1st run.
            //but then change the above URL to https://demo.applitools.com/index_v2.html (for the 2nd run)

            // Create a runner with concurrency of 10
            VisualGridRunner runner = new VisualGridRunner(10);

            // Create Eyes object with the runner, meaning it'll be a Visual Grid eyes.
            Eyes eyes = new Eyes(runner);

            // Get current Eyes configuration object
            Configuration conf = eyes.GetConfiguration();


            //conf.SetApiKey("APPLITOOLS_API_KEY");  // Set the Applitools API KEY here or as an environment variable "APPLITOOLS_API_KEY"
            conf.SetTestName("C# VisualGrid demo")   // Set test name
                .SetAppName("Demo app");             // Set app name

            // Add browsers with different viewports
            conf.AddBrowser(800, 600, BrowserType.CHROME);
            conf.AddBrowser(700, 500, BrowserType.FIREFOX);
            conf.AddBrowser(1200, 800, BrowserType.IE_10);
            conf.AddBrowser(1600, 1200, BrowserType.IE_11);
            conf.AddBrowser(1024, 768, BrowserType.EDGE);
            conf.AddBrowser(800, 600, BrowserType.SAFARI);

            // Add iPhone 4 device emulation in Portrait mode
            conf.AddDeviceEmulation(DeviceName.iPhone_4, ScreenOrientation.Portrait);


            // Set the configuration object to eyes
            eyes.SetConfiguration(conf);

            try
            {
                // Call Open on eyes to initialize a test session
                eyes.Open(webDriver);

                // check the login page
                eyes.Check(Target.Window().Fully().WithName("Login page"));
                webDriver.FindElement(By.Id("log-in")).Click();

                // Check the app page
                eyes.Check(Target.Window().Fully().WithName("App page"));

                // Call Close on eyes to let the server know it should display the results
                eyes.CloseAsync();
            }
            finally
            {
                // Close the browser
                webDriver.Quit();
            }

            // Wait and collect all test results
            TestResultsSummary allTestResults = runner.GetAllTestResults();
            TestContext.Progress.WriteLine(allTestResults);
        }

    }
}
