using Applitools.Selenium;
using Applitools.VisualGrid;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System.Drawing;
using ScreenOrientation = Applitools.VisualGrid.ScreenOrientation;

namespace Applitools.Example.Tests;

/// <summary>
/// Tests for the ACME Bank demo app.
/// </summary>
public class AcmeBankTest
{
    #pragma warning disable CS0162
    #pragma warning disable CS8618

    // Test constants
    public const bool UseUltrafastGrid = true;
    public const bool UseExecutionCloud = false;
    
    // Test control inputs to read once and share for all tests
    private static string? ApplitoolsApiKey;
    private static bool Headless;

    // Applitools objects to share for all tests
    private static BatchInfo Batch;
    private static Configuration Config;
    private static EyesRunner Runner;

    // Test-specific objects
    private WebDriver Driver;
    private Eyes Eyes;

    #pragma warning restore CS8618

    /// <summary>
    /// Sets up the configuration for running visual tests.
    /// The configuration is shared by all tests in a test suite, so it belongs in a `OneTimeSetUp` method.
    /// If you have more than one test class, then you should abstract this configuration to avoid duplication.
    /// <summary>
    [OneTimeSetUp]
    public static void SetUpConfigAndRunner()
    {
        // Read the Applitools API key from an environment variable.
        ApplitoolsApiKey = Environment.GetEnvironmentVariable("APPLITOOLS_API_KEY");

        // Read the headless mode setting from an environment variable.
        // Use headless mode for Continuous Integration (CI) execution.
        // Use headed mode for local development.
        Headless = Environment.GetEnvironmentVariable("HEADLESS")?.ToLower() == "true";

        if (UseUltrafastGrid)
        {
            // Create the runner for the Ultrafast Grid.
            // Concurrency refers to the number of visual checkpoints Applitools will perform in parallel.
            // Warning: If you have a free account, then concurrency will be limited to 1.
            Runner = new VisualGridRunner(new RunnerOptions().TestConcurrency(5));
        }
        else
        {
            // Create the Classic runner for local execution.
            Runner = new ClassicRunner();
        }

        // Create a new batch for tests.
        // A batch is the collection of visual checkpoints for a test suite.
        // Batches are displayed in the Eyes Test Manager, so use meaningful names.
        String runnerName = (UseUltrafastGrid) ? "Ultrafast Grid" : "Classic Runner";
        Batch = new BatchInfo($"Example: Selenium C# NUnit with the {runnerName}");

        // Create a configuration for Applitools Eyes.
        Config = new Configuration();

        // Set the Applitools API key so test results are uploaded to your account.
        // If you don't explicitly set the API key with this call,
        // then the SDK will automatically read the `APPLITOOLS_API_KEY` environment variable to fetch it.
        Config.SetApiKey(ApplitoolsApiKey);

        // Set the batch for the config.
        Config.SetBatch(Batch);

        if (UseUltrafastGrid)
        {
            // Add 3 desktop browsers with different viewports for cross-browser testing in the Ultrafast Grid.
            // Other browsers are also available, like Edge and IE.
            Config.AddBrowser(800, 600, BrowserType.CHROME);
            Config.AddBrowser(1600, 1200, BrowserType.FIREFOX);
            Config.AddBrowser(1024, 768, BrowserType.SAFARI);

            // Add 2 mobile emulation devices with different orientations for cross-browser testing in the Ultrafast Grid.
            // Other mobile devices are available, including iOS.
            Config.AddDeviceEmulation(DeviceName.Pixel_2, ScreenOrientation.Portrait);
            Config.AddDeviceEmulation(DeviceName.Nexus_10, ScreenOrientation.Landscape);
        }
    }

    /// <summary>
    /// Sets up each test with its own ChromeDriver and Applitools Eyes objects.
    /// <summary>
    [SetUp]
    public void OpenBrowserAndEyes()
    {
        // Open the browser with the ChromeDriver instance.
        // Even though this test will run visual checkpoints on different browsers in the Ultrafast Grid,
        // it still needs to run the test one time locally to capture snapshots.
        ChromeOptions options = new ChromeOptions();
        if (Headless) options.AddArgument("headless");
        
        if (UseExecutionCloud)
        {
            // Open the browser remotely in the Execution Cloud.
            Driver = new RemoteWebDriver(new Uri(Eyes.GetExecutionCloudUrl()), options);
        }
        else
        {
            // Create a local WebDriver.
            Driver = new ChromeDriver(options);
        }

        // Set an implicit wait of 10 seconds.
        // For larger projects, use explicit waits for better control.
        // https://www.selenium.dev/documentation/webdriver/waits/
        // The following call works for Selenium 4:
        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

        // Create the Applitools Eyes object connected to the runner and set its configuration.
        Eyes = new Eyes(Runner);
        Eyes.SetConfiguration(Config);
        Eyes.SaveNewTests = true;

        // Open Eyes to start visual testing.
        // It is a recommended practice to set all four inputs:
        Eyes.Open(
            
            // WebDriver object to "watch".
            Driver,
            
            // The name of the application under test.
            // All tests for the same app should share the same app name.
            // Set this name wisely: Applitools features rely on a shared app name across tests.
            "ACME Bank Web App",
            
            // The name of the test case for the given application.
            // Additional unique characteristics of the test may also be specified as part of the test name,
            // such as localization information ("Home Page - EN") or different user permissions ("Login by admin").
            NUnit.Framework.TestContext.CurrentContext.Test.Name,
            
            // The viewport size for the local browser.
            // Eyes will resize the web browser to match the requested viewport size.
            // This parameter is optional but encouraged in order to produce consistent results.
            new Size(1200, 600));
    }

    /// <summary>
    /// This test covers login for the Applitools demo site, which is a dummy banking app.
    /// The interactions use typical Selenium WebDriver calls,
    /// but the verifications use one-line snapshot calls with Applitools Eyes.
    /// If the page ever changes, then Applitools will detect the changes and highlight them in the Eyes Test Manager.
    /// Traditional assertions that scrape the page for text values are not needed here.
    /// <summary>
    [Test]
    public void LogIntoBankAccount()
    {
        // Load the login page.
        Driver.Navigate().GoToUrl("https://demo.applitools.com");

        // Verify the full login page loaded correctly.
        Eyes.Check(Target.Window().Fully().WithName("Login page"));

        // Perform login.
        Driver.FindElement(By.Id("username")).SendKeys("applibot");
        Driver.FindElement(By.Id("password")).SendKeys("I<3VisualTests");
        Driver.FindElement(By.Id("log-in")).Click();

        // Verify the full main page loaded correctly.
        // This snapshot uses LAYOUT match level to avoid differences in closing time text.
        Eyes.Check(Target.Window().Fully().WithName("Main page").Layout());
    }

    /// <summary>
    /// Concludes the test by quitting the browser and closing Eyes.
    /// <summary>
    [TearDown]
    public void CleanUpTest()
    {
        // Close Eyes to tell the server it should display the results.
        Eyes.Close();

        // Quit the WebDriver instance.
        Driver.Quit();
    }

    /// <summary>
    /// Prints the final summary report for the test suite.
    /// <summary>
    [OneTimeTearDown]
    public static void PrintResults()
    {
        // Close the batch and report visual differences to the console.
        // Note that it forces NUnit to wait synchronously for all visual checkpoints to complete.
        TestResultsSummary allTestResults = Runner.GetAllTestResults();
        Console.WriteLine(allTestResults);
    }

    #pragma warning restore CS0162
}
