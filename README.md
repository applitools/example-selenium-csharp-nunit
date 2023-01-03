# Applitools Example: Selenium C# NUnit with the Ultrafast Grid

This is the example project for the [Selenium C# NUnit tutorial](https://applitools.com/tutorials/quickstart/web/selenium/csharp/nunit).
It shows how to start automating visual tests
with [Applitools Eyes](https://applitools.com/platform/eyes/)
and the [Ultrafast Grid](https://applitools.com/platform/ultrafast-grid/)
using [Selenium WebDriver](https://www.selenium.dev/) in C#.

It uses:

* [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) as the programming language
* [Selenium WebDriver](https://www.selenium.dev/) for browser automation
* [Google Chrome](https://www.google.com/chrome/downloads/) as the local browser for testing
* [NuGet](https://www.nuget.org/) for dependency management
* [NUnit](https://nunit.org/) as the core test framework
* [Applitools Eyes](https://applitools.com/platform/eyes/) for visual testing
* [Applitools Ultrafast Grid](https://applitools.com/platform/ultrafast-grid/) for cross-browser execution

To run this example project, you'll need:

1. An [Applitools account](https://auth.applitools.com/users/register), which you can register for free.
2. A good C# editor, such as [Microsoft Visual Studio](https://visualstudio.microsoft.com/)
   or [Visual Studio Code](https://code.visualstudio.com/docs/languages/csharp).
3. The [.NET 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) SDK (which may come bundled with Visual Studio).
4. An up-to-date version of [Google Chrome](https://www.google.com/chrome/downloads/).
5. A corresponding version of [ChromeDriver](https://chromedriver.chromium.org/downloads).

The main test case is [`AcmeBankTest.cs`](Applitools.Example.Tests/AcmeBankTest.cs).

To execute tests, set the `APPLITOOLS_API_KEY` environment variable
to your [account's API key](https://applitools.com/tutorials/guides/getting-started/registering-an-account).

You can run the tests through Test Explorer in Visual Studio,
or you can run tests from the command line using the following `dotnet` commands:

```
dotnet build
dotnet test
```

**For full instructions on running this project, take our
[Selenium C# NUnit tutorial](https://applitools.com/tutorials/quickstart/web/selenium/csharp/nunit)!**