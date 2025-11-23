using Microsoft.Extensions.Configuration;

namespace AutomationTests.Common
{
    public static class ConfigFactory
    {
        private static readonly IConfiguration _config;

        static ConfigFactory()
        {
            // Get the environment variable (e.g., set in CI/CD or via terminal "export ENV=dev")
            var env = Environment.GetEnvironmentVariable("ENV");

            // Build the configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // If an environment is specified (e.g., "dev"), load "appsettings.dev.json" to override values
            if (!string.IsNullOrEmpty(env))
            {
                builder.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
            }

            _config = builder.Build();
        }

        // --- Properties Accessors ---
        public static string ApiBaseUrl => _config["api:baseurl"] 
            ?? throw new InvalidOperationException("Key 'api:baseurl' not found in config.");

        public static string UiBaseUrl => _config["ui:baseurl"] 
            ?? throw new InvalidOperationException("Key 'ui:baseurl' not found in config.");

        public static string CurrentEnv => Environment.GetEnvironmentVariable("ENV") ?? "test (default)";

        public static string Browser
        {
            get
            {
                var envBrowser = Environment.GetEnvironmentVariable("BROWSER");
                if (!string.IsNullOrEmpty(envBrowser))
                {
                    return envBrowser;
                }

                return _config["ui:browser"] ?? "chrome";
            }
        }
    }
}