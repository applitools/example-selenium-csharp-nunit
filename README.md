# Pre-requisites:

1. Visual Studio installed on your machine. Workload ".NET desktop development" should be installed in Visual Studio too (if no - add it with Visual Studio Installer)
   * [Install it from here](https://visualstudio.microsoft.com/downloads/)
2. Chrome browser is installed on your machine.
   
   * [Install Chrome browser](https://support.google.com/chrome/answer/95346?co=GENIE.Platform%3DDesktop&hl=en&oco=0)
3. Chrome Webdriver is on your machine and is in the PATH. Here are some resources from the internet that'll help you.
   * [Download Chrome Webdriver](https://chromedriver.chromium.org/downloads)
   * https://splinter.readthedocs.io/en/0.1/setup-chrome.html
   * https://stackoverflow.com/questions/38081021/using-selenium-on-mac-chrome
   * https://www.youtube.com/watch?time_continue=182&v=dz59GsdvUF8
4. Git is installed on your machine. 

   * [Install git](https://www.atlassian.com/git/tutorials/install-git)
5. Restart your machine to implement updated  environment variables (need for some OS).

# Steps to run this example

1. Git clone this repo
   
    * `git clone https://github.com/applitools/tutorial-selenium-csharp-ultrafastgrid.git`
    
2. Get your API key to set it in code (or in the APPLITOOLS_API_KEY environment variable).

    * You can get your API key by logging into Applitools > Person Icon > My API Key.

4. Navigate to folder `tutorial-selenium-csharp-ultrafastgrid` and double click the `ApplitoolsTutorial.sln`. This will open the project in Visual Studio

5. In Visual Studio open file UFGDemo.cs and change the `APPLITOOLS_API_KEY` with your own in code.
   Set your ApiKey in string 'config.SetApiKey("...") ' (or comment the string and set APPLITOOLS_API_KEY environment variable)
   
6. In Visual Studio open Package Manager Console (Tools > NuGet Package Manager > Package Manager Console) and enter command `dotnet restore` in the console. Command execution can take several minutes.

6. Build the solution (Build > Build Solution). It can take several minutes.

7. Run project (Debug > Start Without Debugging). Execution of project can take several minutes.

8. If needed, in case of some problems - update package Eyes.Selenium by NuGet Package Manager -  Tools > NuGet Package Manager > Manage Nuget Packages for Solution, tab Updates. Select package for update (Eyes.Selenium), select needed version in right panel and tap Install

    ![](NuGet.png)