using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Zadanie1.Tests.PageObjects;

namespace Zadanie1.Tests.Tests;

public class ShoppingTest : PageTest
{
    private LoginPage _loginPage = null!;
    
    [SetUp]
    public async Task OpenLoginPage()
    {
        _loginPage = new LoginPage(Page);
        await _loginPage.OpenAsync();
    }
    
    // Dane kupującego są dowolne - saucedemo ich nie weryfikuje, sprawdza tylko,
    // czy pola nie są puste.
    private const string FirstName = "Jan";
    private const string LastName = "Kowalski";
    private const string PostalCode = "00-001";

    [Test]
    public async Task Buying_SuccessPage_ShowsOrderConfirmation()
    {
        var inventory = await _loginPage.LoginAsync(TestUsers.Standard, TestUsers.Password);
        await inventory.AddToCartAsync("sauce-labs-backpack");

        var cart = await inventory.OpenCartAsync();
        await Expect(cart.Header).ToHaveTextAsync("Your Cart");
        await Expect(cart.ItemNames).ToHaveTextAsync(new[] { "Sauce Labs Backpack" });

        var customerInfo = await cart.CheckoutAsync();
        var overview = await customerInfo.ContinueAsync(FirstName, LastName, PostalCode);
        
        await Expect(overview.Header).ToHaveTextAsync("Checkout: Overview");
        await Expect(overview.ItemNames).ToHaveTextAsync(new[] { "Sauce Labs Backpack" });
        await Expect(overview.TotalLabel).ToContainTextAsync("Total: $");

        var complete = await overview.FinishAsync();

        await Expect(complete.Header).ToHaveTextAsync("Checkout: Complete!");
        await Expect(complete.ConfirmationHeader).ToHaveTextAsync("Thank you for your order!");
    }
}