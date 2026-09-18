using Microsoft.Playwright;

namespace Zadanie1.Tests.PageObjects;

/// <summary>Potwierdzenie zamówienia - "Checkout: Complete!".</summary>
public class CheckoutCompletePage : BasePage
{
    public CheckoutCompletePage(IPage page) : base(page) { }

    protected override string Path => "/checkout-complete.html";

    public ILocator Header => Page.Locator("[data-test=title]");

    /// <summary>"Thank you for your order!" - właściwa asercja sukcesu.</summary>
    public ILocator ConfirmationHeader => Page.Locator("[data-test=complete-header]");

    /// <summary>Tekst pod nagłówkiem, o wysyłce zamówienia.</summary>
    public ILocator ConfirmationText => Page.Locator("[data-test=complete-text]");

    private ILocator BackHomeButton => Page.Locator("[data-test=back-to-products]");

    public async Task WaitUntilLoadedAsync()
    {
        await Page.WaitForURLAsync($"**{Path}");
        await ConfirmationHeader.WaitForAsync();
    }

    /// <summary>"Back Home" - wraca na listę produktów.</summary>
    public async Task<InventoryPage> BackHomeAsync()
    {
        await BackHomeButton.ClickAsync();

        var inventory = new InventoryPage(Page);
        await inventory.WaitUntilLoadedAsync();
        return inventory;
    }
}
