using Microsoft.Playwright;
using AutomationTests.Ui.Data;
using AutomationTests.Common;
using static Microsoft.Playwright.Assertions;

namespace AutomationTests.Ui.Pages
{
    public class PracticeFormPage
    {
        private readonly IPage _page;

        private const string FirstNameInput = "#firstName";
        private const string LastNameInput = "#lastName";
        private const string EmailInput = "#userEmail";
        private const string MobileInput = "#userNumber";
        private const string AddressInput = "#currentAddress";
        private const string SubmitButton = "#submit";
        private const string DobInput = "#dateOfBirthInput";
        private const string DobYearSelect = ".react-datepicker__year-select";
        private const string DobMonthSelect = ".react-datepicker__month-select";
        private const string SubjectsInput = "#subjectsInput";
        private const string StateDropdown = "#state";
        private const string CityDropdown = "#city";

        // Error Locators
        private const string GenderMaleError = "label[for='gender-radio-1'].custom-control-label";
        private const string GenderFemaleError = "label[for='gender-radio-2'].custom-control-label";
        private const string GenderOtherError = "label[for='gender-radio-3'].custom-control-label";

        // Constructor injection via DI or Factory
        public PracticeFormPage(IPage page)
        {
            _page = page;
        }

        public async Task Navigate()
        {
            await _page.GotoAsync(UiConstants.PracticeFormUrl);
        }

        public async Task FillInForm(PracticeFormData data)
        {
            await _page.FillAsync(FirstNameInput, data.FirstName);
            await _page.FillAsync(LastNameInput, data.LastName);
            await _page.FillAsync(EmailInput, data.Email);
            await _page.FillAsync(MobileInput, data.Mobile);
            await _page.FillAsync(AddressInput, data.Address);

            await SelectGender(data.Gender);
            await SelectDateOfBirth(data.BirthDay, data.BirthMonth, data.BirthYear);
            await SetSubject(data.Subject);

            foreach (var hobby in data.Hobbies)
            {
                await SelectHobby(hobby);
            }

            await SelectStateAndCity(data.State, data.City);
        }

        public async Task SelectGender(string gender)
        {
            var genderLocator = $"//label[normalize-space()='{gender}']";
            await _page.Locator(genderLocator).ClickAsync();
        }

        public async Task SelectDateOfBirth(string day, string month, string year)
        {
            await _page.ClickAsync(DobInput);
            await _page.SelectOptionAsync(DobYearSelect, year);
            await _page.SelectOptionAsync(DobMonthSelect, month);

            // Formatting the day to 3 digits (e.g., 1 -> 001) to match the class name
            var dayInt = int.Parse(day);
            var dayLocator = $".react-datepicker__day--{dayInt:D3}";

            // We specifically exclude days from the "previous month" that might be visible
            await _page.Locator(dayLocator).First.ClickAsync();
        }

        public async Task SetSubject(string subject)
        {
            var subjectLocator = _page.Locator(SubjectsInput);
            await subjectLocator.ClickAsync();

            await subjectLocator.PressSequentiallyAsync(subject, new LocatorPressSequentiallyOptions { Delay = 50 });

            await _page.PressAsync(SubjectsInput, "Enter");
        }

        public async Task SelectHobby(string hobby)
        {
            var hobbyLocator = $"//label[normalize-space()='{hobby}']";
            await _page.Locator(hobbyLocator).ClickAsync();
        }

        public async Task SelectStateAndCity(string state, string city)
        {
            await _page.Locator(StateDropdown).ClickAsync(new LocatorClickOptions { Force = true });

            await _page.Locator($"{StateDropdown} input").FillAsync(state);
            await _page.Keyboard.PressAsync("Enter");

            await _page.Locator(CityDropdown).WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });

            await Task.Delay(500);

            await _page.Locator(CityDropdown).ClickAsync(new LocatorClickOptions { Force = true });

            await _page.Locator($"{CityDropdown} input").FillAsync(city);
            await _page.Keyboard.PressAsync("Enter");
        }

        public async Task Submit()
        {
            // "Force = true" ensures click happens even if element is slightly covered/animating
            await _page.ClickAsync(SubmitButton, new PageClickOptions { Force = true });
        }

        // --- ASSERTIONS ---
        public async Task AssertFirstNameIsInvalid()
        {
            await Expect(_page.Locator(FirstNameInput)).ToHaveCSSAsync("border-color", "rgb(220, 53, 69)");
        }

        public async Task AssertLastNameIsInvalid()
        {
            await Expect(_page.Locator(LastNameInput)).ToHaveCSSAsync("border-color", "rgb(220, 53, 69)");
        }

        public async Task AssertMobileIsInvalid()
        {
            await Expect(_page.Locator(MobileInput)).ToHaveCSSAsync("border-color", "rgb(220, 53, 69)");
        }

        public async Task AssertGenderIsInvalid()
        {
            await Expect(_page.Locator(GenderMaleError)).ToHaveCSSAsync("color", "rgb(220, 53, 69)");
            await Expect(_page.Locator(GenderFemaleError)).ToHaveCSSAsync("color", "rgb(220, 53, 69)");
            await Expect(_page.Locator(GenderOtherError)).ToHaveCSSAsync("color", "rgb(220, 53, 69)");
        }
    }
}