using Reqnroll;
using FluentAssertions;
using Microsoft.Playwright;
using AutomationTests.Ui.Data;
using AutomationTests.Ui.Pages;

namespace AutomationTests.Ui.Steps
{
    [Binding]
    public class UiStepDefinitions
    {
        // Page Objects
        private readonly PracticeFormPage _formPage;
        private readonly ConfirmationModal _modal;
        
        private PracticeFormData _formData;

        public UiStepDefinitions(IPage page)
        {
            _formPage = new PracticeFormPage(page);
            _modal = new ConfirmationModal(page);
        }

        [Given("I navigate to the practice form page")]
        public async Task GivenINavigateToThePracticeFormPage()
        {
            await _formPage.Navigate();
        }

        [When("I fill the registration form with {string} data")]
        public async Task WhenIFillTheRegistrationFormWithData(string dataType)
        {
            if (dataType.ToLower() == "default")
            {
                _formData = PracticeFormData.GetDefaultData();
            }
            else
            {
                _formData = PracticeFormData.GetRandomData();
            }

            Console.WriteLine("Test Data: " + System.Text.Json.JsonSerializer.Serialize(_formData));

            await _formPage.FillInForm(_formData);
        }

        [When("I submit the form")]
        public async Task WhenISubmitTheForm()
        {
            await _formPage.Submit();
        }

        [Then("I should see the confirmation modal")]
        public async Task ThenIShouldSeeTheConfirmationModal()
        {
            await _modal.WaitForModal();
            var title = await _modal.GetModalTitle();
            title.Should().Be("Thanks for submitting the form");
        }

        [Then("I verify the {string} data in the modal")]
        public async Task ThenIVerifyTheDataInTheModal(string dataType)
        {
            var expectedName = $"{_formData.FirstName} {_formData.LastName}";
            var expectedDate = $"{_formData.BirthDay} {_formData.BirthMonth},{_formData.BirthYear}";
            var expectedHobbies = string.Join(", ", _formData.Hobbies);
            var expectedStateAndCity = $"{_formData.State} {_formData.City}";

            // Assertions (Using FluentAssertions with "Because" for better error messages)
            (await _modal.GetValueForLabel("Student Name")).Should().Be(expectedName, "Student Name mismatch");
            (await _modal.GetValueForLabel("Student Email")).Should().Be(_formData.Email, "Email mismatch");
            (await _modal.GetValueForLabel("Gender")).Should().Be(_formData.Gender, "Gender mismatch");
            (await _modal.GetValueForLabel("Mobile")).Should().Be(_formData.Mobile, "Mobile mismatch");
            (await _modal.GetValueForLabel("Date of Birth")).Should().Be(expectedDate, "Date of Birth mismatch");
            (await _modal.GetValueForLabel("Subjects")).Should().Be(_formData.Subject, "Subjects mismatch");
            (await _modal.GetValueForLabel("Hobbies")).Should().Be(expectedHobbies, "Hobbies mismatch");
            (await _modal.GetValueForLabel("Address")).Should().Be(_formData.Address, "Address mismatch");
            (await _modal.GetValueForLabel("State and City")).Should().Be(expectedStateAndCity, "State/City mismatch");
        }

        [When("I close the modal")]
        public async Task WhenICloseTheModal()
        {
            await _modal.Close();
        }

        [Then("the modal should disappear")]
        public async Task ThenTheModalShouldDisappear()
        {
            await _modal.AssertModalIsClosed();
        }

        [When("I submit the empty form")]
        public async Task WhenISubmitTheEmptyForm()
        {
            await _formPage.Submit();
        }

        [Then("I should see validation errors for required fields")]
        public async Task ThenIShouldSeeValidationErrors()
        {
            await _formPage.AssertFirstNameIsInvalid();
            await _formPage.AssertLastNameIsInvalid();
            await _formPage.AssertGenderIsInvalid();
            await _formPage.AssertMobileIsInvalid();
        }

        [When("I fill the complete form with data: {string}, {string}, {string}, {string}, {string}, {string}, {string}, {string}, {string}, {string}, {string}, {string}, {string}")]
        public async Task WhenIFillTheCompleteFormWithData(
            string fName, string lName, string email, string gender, string mobile, 
            string day, string month, string year, string subject, string hobby, 
            string address, string state, string city)
        {
            _formData = new PracticeFormData
            {
                FirstName = fName,
                LastName = lName,
                Email = email,
                Gender = gender,
                Mobile = mobile,
                BirthDay = day,
                BirthMonth = month,
                BirthYear = year,
                Subject = subject,
                Hobbies = new[] { hobby }, 
                Address = address,
                State = state,
                City = city
            };

            await _formPage.FillInForm(_formData);
        }

        [Then("I verify the complete modal data matches: {string}, {string}, {string}, {string}, {string}, {string}, {string}, {string}, {string}, {string}, {string}, {string}, {string}")]
        public async Task ThenIVerifyTheCompleteModalDataMatches(
             string fName, string lName, string email, string gender, string mobile,
             string day, string month, string year, string subject, string hobby,
             string address, string state, string city)
        {
            var expectedName = $"{fName} {lName}";
            var expectedDate = $"{day} {month},{year}";
            var expectedStateAndCity = $"{state} {city}";

            (await _modal.GetValueForLabel("Student Name")).Should().Be(expectedName);
            (await _modal.GetValueForLabel("Student Email")).Should().Be(email);
            (await _modal.GetValueForLabel("Gender")).Should().Be(gender);
            (await _modal.GetValueForLabel("Mobile")).Should().Be(mobile);
            (await _modal.GetValueForLabel("Date of Birth")).Should().Be(expectedDate);
            (await _modal.GetValueForLabel("Subjects")).Should().Be(subject);
            (await _modal.GetValueForLabel("Hobbies")).Should().Be(hobby);
            (await _modal.GetValueForLabel("Address")).Should().Be(address);
            (await _modal.GetValueForLabel("State and City")).Should().Be(expectedStateAndCity);
        }
    }
}