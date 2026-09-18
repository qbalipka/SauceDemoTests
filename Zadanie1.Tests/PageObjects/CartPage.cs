using Microsoft.Playwright;

namespace Zadanie1.Tests.PageObjects;

/// <summary>Koszyk - podsumowanie pozycji przed rozpoczęciem zamówienia.</summary>
public class CartPage : BasePage
{
    public CartPage(IPage page) : base(page) { }

    protected override string Path => "/cart.html";

    public ILocator Header => Page.Locator("[data-test=title]");
    public ILocator Items => Page.Locator("[data-test=inventory-item]");
    public ILocator ItemNames => Page.Locator("[data-test=inventory-item-name]");
    public ILocator CartBadge => Page.Locator("[data-test=shopping-cart-badge]");

    private ILocator CheckoutButton => Page.Locator("[data-test=checkout]");
    private ILocator ContinueShoppingButton => Page.Locator("[data-test=continue-shopping]");

    /// <summary>
    /// Czeka na adres i na listę koszyka, a nie na pojedynczą pozycję -
    /// pusty koszyk też jest poprawnym stanem tej strony.
    /// </summary>
    public async Task WaitUntilLoadedAsync()
    {
        await Page.WaitForURLAsync($"**{Path}");
        await Page.Locator("[data-test=cart-list]").WaitForAsync();
    }

    public Task<int> CountItemsAsync() => Items.CountAsync();

    /// <summary>Usuwa pozycję z koszyka, np. RemoveAsync("sauce-labs-backpack").</summary>
    public Task RemoveAsync(string productSlug) =>
        Page.Locator($"[data-test=remove-{productSlug}]").ClickAsync();

    /// <summary>Wraca na listę produktów.</summary>
    public async Task<InventoryPage> ContinueShoppingAsync()
    {
        await ContinueShoppingButton.ClickAsync();

        var inventory = new InventoryPage(Page);
        await inventory.WaitUntilLoadedAsync();
        return inventory;
    }

    /// <summary>Przechodzi do pierwszego kroku zamówienia (dane kupującego).</summary>
    public async Task<CheckoutStepOnePage> CheckoutAsync()
    {
        await CheckoutButton.ClickAsync();

        var stepOne = new CheckoutStepOnePage(Page);
        await stepOne.WaitUntilLoadedAsync();
        return stepOne;
    }
}
