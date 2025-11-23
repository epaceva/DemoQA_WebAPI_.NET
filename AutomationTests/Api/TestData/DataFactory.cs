using AutomationTests.Api.Data; 

namespace AutomationTests.Api.TestData
{
    public static class DataFactory
    {
        public static UserCreateRequest CreateDefaultUser()
        {
            return new UserCreateRequest
            {
                Name = "Test User",
                Job = "Automation Tester"
            };
        }
    }
}