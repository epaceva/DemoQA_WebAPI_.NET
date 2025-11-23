using System.Net;
using System.Text.Json;
using Reqnroll;
using RestSharp;
using FluentAssertions;
using AutomationTests.Api.Data;
using AutomationTests.Api.TestData;
using AutomationTests.Common; 

namespace AutomationTests.Api.Steps
{
    [Binding]
    public class ApiStepDefinitions
    {
        private readonly RestClient _client;
        private RestResponse? _response; 
        private UserCreateRequest? _userRequest; 

        public ApiStepDefinitions()
        {
            var baseUrl = ApiConstants.BaseUrl;
            var options = new RestClientOptions(baseUrl);
            _client = new RestClient(options);
            _client.AddDefaultHeader(ApiConstants.HeaderName, ApiConstants.HeaderValue);
        }

        [When("I send POST Request to create user")]
        public async Task WhenISendPOSTRequestToCreateUser()
        {
            _userRequest = DataFactory.CreateDefaultUser();

            var requestUrl = $"{ApiConstants.BasePath}{ApiConstants.UsersEndpoint}";
            var request = new RestRequest(requestUrl, Method.Post);
            request.AddJsonBody(_userRequest);

            _response = await _client.ExecuteAsync(request);

            Console.WriteLine($"Sent POST to {ApiConstants.BaseUrl}{requestUrl}");
            Console.WriteLine($"Response Status: {_response.StatusCode}");
        }

        [Then("I get response with status code {int}")]
        public void ThenIGetResponseWithStatusCode(int expectedStatusCode)
        {
            // Ensure response is not null before checking status
            _response.Should().NotBeNull();
            _response!.StatusCode.Should().Be((HttpStatusCode)expectedStatusCode);
        }

        [Then("I verify {string} and {string} in the response")]
        public void ThenIVerifyAndInTheResponse(string ignoredJob, string ignoredName)
        {
            _response.Should().NotBeNull();
            
            var content = _response!.Content ?? "{}"; 

            var responseBody = JsonSerializer.Deserialize<UserCreateResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            responseBody.Should().NotBeNull();
            _userRequest.Should().NotBeNull();

            responseBody!.Name.Should().Be(_userRequest!.Name);
            responseBody.Job.Should().Be(_userRequest.Job);
        }

        [Then("I verify {string} and {string} are generated")]
        public void ThenIVerifyAndAreGenerated(string ignoredId, string ignoredCreatedAt)
        {
            _response.Should().NotBeNull();
            var content = _response!.Content ?? "{}";

            var responseBody = JsonSerializer.Deserialize<UserCreateResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            responseBody.Should().NotBeNull();
            responseBody!.Id.Should().NotBeNull();
            responseBody.Id.Should().BeGreaterThan(0);
            responseBody.CreatedAt.Should().NotBeNullOrEmpty();
        }
    }
}