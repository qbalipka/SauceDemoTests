using Microsoft.Playwright;

namespace Zadanie1.Tests.PageObjects;

/// <summary>Lista produktów po zalogowaniu.</summary>
public class InventoryPage : BasePage
{
    public InventoryPage(IPage page) : base(page) { }

    protected override string Path => "/inventory.html";

    public ILocator Header => Page.Locator(".title");
    public ILocator Products => Page.Locator("[data-test=inventory-item]");
    public ILocator ProductNames => Page.Locator("[data-test=inventory-item-name]");
    public ILocator CartBadge => Page.Locator(".shopping_cart_badge");

    /// <summary>
    /// Czeka na adres i na faktyczny element listy. Sprawdzanie samego stanu
    /// ładowania dawałoby fałszywe zielone, gdyby serwis zwrócił inną stronę.
    /// </summary>
    public async Task WaitUntilLoadedAsync()
    {
        await Page.WaitForURLAsync($"**{Path}");
        await Products.First.WaitForAsync();
    }

    /// <summary>Dodaje produkt do koszyka, np. AddToCartAsync("sauce-labs-backpack").</summary>
    public Task AddToCartAsync(string productSlug) =>
        Page.Locator($"[data-test=add-to-cart-{productSlug}]").ClickAsync();

    public Task<int> CountProductsAsync() => Products.CountAsync();

    /// <summary>Klika ikonę koszyka i zwraca stronę koszyka.</summary>
    public async Task<CartPage> OpenCartAsync()
    {
        await Page.Locator("[data-test=shopping-cart-link]").ClickAsync();

        var cart = new CartPage(Page);
        await cart.WaitUntilLoadedAsync();
        return cart;
    }
}
