namespace AutomationTests.Common
{
    public static class ApiConstants
    {
        // DYNAMIC CONFIGURATION:
        public static string BaseUrl => ConfigFactory.ApiBaseUrl;

        // STATIC CONSTANTS:
        public const string BasePath = "/api";
        public const string UsersEndpoint = "/users";

        // HEADERS:
        public const string HeaderName = "x-api-key";
        public const string HeaderValue = "reqres-free-v1";
    }
}