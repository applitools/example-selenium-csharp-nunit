using Applitools;
using Applitools.Selenium;
using Applitools.VisualGrid;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Drawing;
using Configuration = Applitools.Selenium.Configuration;
using ScreenOrientation = Applitools.VisualGrid.ScreenOrientation;

namespace ApplitoolsTutorial
{
	[TestFixture]
    public class UFGDemo
    {
		private EyesRunner runner;
        private Eyes eyes;
        private IWebDriver driver;

		[SetUp]
		public void BeforeEach() {
			var CI = Environment.GetEnvironmentVariable("CI");
            var options = new ChromeOptions();
            if(CI != null) {
                options.AddArguments("headless");
            } 
            // Use Chrome browser
            driver = new ChromeDriver(options);

            //Initialize the Runner for your test with concurrency of 5.
			// Create Eyes object with the runner, meaning it'll be a Visual Grid eyes.
            runner = new VisualGridRunner(new RunnerOptions().TestConcurrency(5));

            // Initialize the eyes SDK (IMPORTANT: make sure your API key is set in the APPLITOOLS_API_KEY env variable).
            eyes = new Eyes(runner);

			// Initialize eyes Configuration
			Configuration config = new Configuration();

			// You can get your api key from the Applitools dashboard
			config.SetApiKey(Environment.GetEnvironmentVariable("APPLITOOLS_API_KEY"));

			// create a new batch info instance and set it to the configuration
			config.SetBatch(new BatchInfo("Ultrafast Batch"));

			// Add browsers with different viewports
			config.AddBrowser(800, 600, BrowserType.CHROME);
			config.AddBrowser(700, 500, BrowserType.FIREFOX);
			config.AddBrowser(1600, 1200, BrowserType.IE_11);
			config.AddBrowser(1024, 768, BrowserType.EDGE_CHROMIUM);
			config.AddBrowser(800, 600, BrowserType.SAFARI);

			// Add mobile emulation devices in Portrait mode
			config.AddDeviceEmulation(DeviceName.iPhone_X, ScreenOrientation.Portrait);
			config.AddDeviceEmulation(DeviceName.Pixel_2, ScreenOrientation.Portrait);

			// Set the configuration object to eyes
			eyes.SetConfiguration(config);

		}

		[Test]
		public void UFGTest(){
			            // Start the test by setting AUT's name, window or the page name that's being tested, viewport width and height
            eyes.Open(driver, "Demo App - csharp ufg", "Smoke Test", new Size(800, 600));

            // Navigate the browser to the "ACME" demo app. To see visual bugs after the first run, use the commented line below instead.
            driver.Url = "https://demo.applitools.com/";
            //driver.Url = "https://demo.applitools.com/index_v2.html";

            // Visual checkpoint #1 - Check the login page. using the fluent API
            // https://applitools.com/docs/topics/sdk/the-eyes-sdk-check-fluent-api.html?Highlight=fluent%20api
            eyes.Check(Target.Window().Fully().WithName("Login Window"));

            // This will create a test with two test steps.
            driver.FindElement(By.Id("log-in")).Click();
            
            // Visual checkpoint #2 - Check the app page.
            eyes.Check(Target.Window().Fully().WithName("App Window"));

            // End the test.
            eyes.CloseAsync();
		}

		[TearDown]
		public void AfrerEach(){
			// Close the browser.
            driver.Quit();

            // If the test was aborted before eyes.close was called, ends the test as aborted.
            eyes.AbortIfNotClosed();

            //Wait and collect all test results
            // we pass false to this method to suppress the exception that is thrown if we
            // find visual differences
            TestResultsSummary allTestResults = runner.GetAllTestResults();

            // Print results
            Console.WriteLine(allTestResults);
		}

	}
}
