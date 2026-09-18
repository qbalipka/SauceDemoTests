using Microsoft.Playwright;

namespace Zadanie1.Tests.PageObjects;

/// <summary>Krok 2 zamówienia - "Checkout: Overview" z podsumowaniem kwot.</summary>
public class CheckoutStepTwoPage : BasePage
{
    public CheckoutStepTwoPage(IPage page) : base(page) { }

    protected override string Path => "/checkout-step-two.html";

    public ILocator Header => Page.Locator("[data-test=title]");
    public ILocator Items => Page.Locator("[data-test=inventory-item]");
    public ILocator ItemNames => Page.Locator("[data-test=inventory-item-name]");

    /// <summary>Etykiety z kwotami, np. "Item total: $29.99" / "Tax: $2.40" / "Total: $32.39".</summary>
    public ILocator SubtotalLabel => Page.Locator("[data-test=subtotal-label]");
    public ILocator TaxLabel => Page.Locator("[data-test=tax-label]");
    public ILocator TotalLabel => Page.Locator("[data-test=total-label]");

    public ILocator PaymentInfo => Page.Locator("[data-test=payment-info-value]");
    public ILocator ShippingInfo => Page.Locator("[data-test=shipping-info-value]");

    private ILocator FinishButton => Page.Locator("[data-test=finish]");
    private ILocator CancelButton => Page.Locator("[data-test=cancel]");

    /// <summary>Czeka na adres i na etykietę sumy - kwoty liczą się po stronie klienta.</summary>
    public async Task WaitUntilLoadedAsync()
    {
        await Page.WaitForURLAsync($"**{Path}");
        await TotalLabel.WaitForAsync();
    }

    public Task<int> CountItemsAsync() => Items.CountAsync();

    /// <summary>Składa zamówienie i przechodzi na stronę potwierdzenia.</summary>
    public async Task<CheckoutCompletePage> FinishAsync()
    {
        await FinishButton.ClickAsync();

        var complete = new CheckoutCompletePage(Page);
        await complete.WaitUntilLoadedAsync();
        return complete;
    }

    /// <summary>Rezygnacja - wraca na listę produktów (nie do koszyka).</summary>
    public async Task<InventoryPage> CancelAsync()
    {
        await CancelButton.ClickAsync();

        var inventory = new InventoryPage(Page);
        await inventory.WaitUntilLoadedAsync();
        return inventory;
    }
}
