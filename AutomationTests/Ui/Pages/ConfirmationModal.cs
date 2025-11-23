using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions; 

namespace AutomationTests.Ui.Pages
{
    public class ConfirmationModal
    {
        // All Playwright actions will be performed using this object.
        private readonly IPage _page;

        // Locators are stored as private const strings.
        private const string ModalRoot = ".modal-content";
        private const string ModalTitle = "#example-modal-sizes-title-lg";
        private const string CloseButton = "#closeLargeModal";

        private const string TableValueTemplate = "//td[text()='{0}']/following-sibling::td";

        public ConfirmationModal(IPage page)
        {
            _page = page;
        }

        public async Task WaitForModal()
        {
            await _page.Locator(ModalRoot).WaitForAsync();
        }

        public async Task<string> GetModalTitle()
        {
            // TextContentAsync or InnerTextAsync works here
            return await _page.Locator(ModalTitle).TextContentAsync() ?? string.Empty;
        }

        public async Task<string> GetValueForLabel(string label)
        {
            // Formatting the dynamic XPath
            string locator = string.Format(TableValueTemplate, label);
            return await _page.Locator(locator).TextContentAsync() ?? string.Empty;
        }

        public async Task Close()
        {
            // Force = true helps if the modal animation isn't perfectly finished
            await _page.ClickAsync(CloseButton, new PageClickOptions { Force = true });
        }

        public async Task AssertModalIsClosed()
        {
            // Using Playwright Assertions (Expect)
            await Expect(_page.Locator(ModalRoot)).ToBeHiddenAsync();
        }
    }
}