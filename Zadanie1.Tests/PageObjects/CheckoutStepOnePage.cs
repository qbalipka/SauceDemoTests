using Microsoft.Playwright;

namespace Zadanie1.Tests.PageObjects;

/// <summary>Krok 1 zamówienia - "Checkout: Your Information".</summary>
public class CheckoutStepOnePage : BasePage
{
    public CheckoutStepOnePage(IPage page) : base(page) { }

    protected override string Path => "/checkout-step-one.html";

    public ILocator Header => Page.Locator("[data-test=title]");

    /// <summary>Komunikat walidacji nad formularzem - publiczny, bo asercje robimy w testach.</summary>
    public ILocator ErrorMessage => Page.Locator("[data-test=error]");

    private ILocator FirstNameInput => Page.Locator("[data-test=firstName]");
    private ILocator LastNameInput => Page.Locator("[data-test=lastName]");
    private ILocator PostalCodeInput => Page.Locator("[data-test=postalCode]");
    private ILocator ContinueButton => Page.Locator("[data-test=continue]");
    private ILocator CancelButton => Page.Locator("[data-test=cancel]");

    public async Task WaitUntilLoadedAsync()
    {
        await Page.WaitForURLAsync($"**{Path}");
        await FirstNameInput.WaitForAsync();
    }

    /// <summary>Wypełnia formularz i zatwierdza, bez oczekiwania na wynik.</summary>
    public async Task SubmitCustomerInfoAsync(string firstName, string lastName, string postalCode)
    {
        await FirstNameInput.FillAsync(firstName);
        await LastNameInput.FillAsync(lastName);
        await PostalCodeInput.FillAsync(postalCode);
        await ContinueButton.ClickAsync();
    }

    /// <summary>Wypełnia formularz i przechodzi do podsumowania. Dla poprawnych danych.</summary>
    public async Task<CheckoutStepTwoPage> ContinueAsync(string firstName, string lastName, string postalCode)
    {
        await SubmitCustomerInfoAsync(firstName, lastName, postalCode);

        var stepTwo = new CheckoutStepTwoPage(Page);
        await stepTwo.WaitUntilLoadedAsync();
        return stepTwo;
    }

    /// <summary>Rezygnacja - wraca do koszyka.</summary>
    public async Task<CartPage> CancelAsync()
    {
        await CancelButton.ClickAsync();

        var cart = new CartPage(Page);
        await cart.WaitUntilLoadedAsync();
        return cart;
    }
}
