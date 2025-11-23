namespace AutomationTests.Common
{
    public static class UiConstants
    {
        // Using the ConfigFactory to get the base URL (e.g. demoqa.com) and appending the path
        public static string PracticeFormUrl => $"{ConfigFactory.UiBaseUrl}/automation-practice-form";
    }
}