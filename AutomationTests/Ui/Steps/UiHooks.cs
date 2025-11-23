using Reqnroll.BoDi;
using Microsoft.Playwright;
using NUnit.Framework;
using Reqnroll;
using AutomationTests.Common;

namespace AutomationTests.Ui.Steps
{
    [Binding]
    public class UiHooks
    {
        private readonly IObjectContainer _objectContainer;
        
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IBrowserContext _context;
        private IPage _page;

        public UiHooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario("UI")]
        public async Task SetupBrowser()
        {
            // 1. Initialize Playwright
            _playwright = await Playwright.CreateAsync();

            // 2. Get browser type from configuration (Environment variable or appsettings.json)
            var browserType = ConfigFactory.Browser.ToLower();

            // 3. Setup launch options (Headless mode, SlowMo, Arguments)
            var options = new BrowserTypeLaunchOptions
            {
                Headless = false, // Set to true for CI/CD execution
                SlowMo = 100,     // Slow down execution slightly to see actions
                Args = new[] { "--start-maximized" } // Arguments for Chromium-based browsers
            };

            // Log current setup to console
            Console.WriteLine($"🚀 RUNNING ON ENVIRONMENT: {ConfigFactory.CurrentEnv.ToUpper()} | BROWSER: {browserType.ToUpper()}");

            // 4. Launch the appropriate browser based on configuration
            switch (browserType)
            {
                case "firefox":
                    _browser = await _playwright.Firefox.LaunchAsync(options);
                    break;
                case "webkit": // Safari engine
                    _browser = await _playwright.Webkit.LaunchAsync(options);
                    break;
                case "chrome":
                case "chromium":
                default:
                    // Chrome requires a specific channel to emulate the real browser
                    options.Channel = "chrome"; 
                    _browser = await _playwright.Chromium.LaunchAsync(options);
                    break;
            }

            // 5. Create a new Browser Context (isolated session)
            _context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
            });

            // 6. Create a new Page (Tab)
            _page = await _context.NewPageAsync();
            
            // Set default timeout to 30 seconds to handle network delays
            _page.SetDefaultTimeout(30000);

            // --- AD BLOCKER LOGIC ---
            // Intercept and abort requests to known ad servers to improve stability
            await _page.RouteAsync("**/*", async route =>
            {
                var url = route.Request.Url;
                if (url.Contains("googleads") || 
                    url.Contains("googlesyndication") || 
                    url.Contains("adservice") ||
                    url.Contains("doubleclick"))
                {
                    await route.AbortAsync();
                }
                else
                {
                    await route.ContinueAsync();
                }
            });

            // Inject CSS to hide sticky banners and footers that might intercept clicks
            await _page.AddInitScriptAsync(@"
                const style = document.createElement('style');
                style.innerHTML = `
                    #fixedban { display: none !important; }
                    footer { display: none !important; }
                    #ad { display: none !important; }
                `;
                document.head.appendChild(style);
            ");

            // 7. Register the Page instance in the DI container for Step Definitions to use
            _objectContainer.RegisterInstanceAs(_page);
        }

        [AfterScenario("UI")]
        public async Task TearDownBrowser()
        {
            // Check if the test failed
            var outcome = TestContext.CurrentContext.Result.Outcome.Status;

            if (outcome == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                if (_page != null)
                {
                    try
                    {
                        // Take a screenshot on failure
                        var screenshotBytes = await _page.ScreenshotAsync(new PageScreenshotOptions { FullPage = true });
                        
                        // Save screenshot to disk
                        var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, $"ERROR_{TestContext.CurrentContext.Test.Name}.png");
                        await File.WriteAllBytesAsync(path, screenshotBytes);
                        
                        // Attach screenshot to NUnit report
                        TestContext.AddTestAttachment(path);
                        
                        Console.WriteLine($"Screenshot saved to: {path}");
                    }
                    catch { }
                }
            }

            // Cleanup resources
            if (_page != null) await _page.CloseAsync();
            if (_browser != null) await _browser.CloseAsync();
            _playwright?.Dispose();
        }
    }
}