using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Zadanie1.Tests.PageObjects;

namespace Zadanie1.Tests.Tests;

[TestFixture]
public class LoginTests : PageTest
{
    private LoginPage _loginPage = null!;
    
    [SetUp]
    public async Task OpenLoginPage()
    {
        _loginPage = new LoginPage(Page);
        await _loginPage.OpenAsync();
    }

    [Test]
    public async Task Login_WithValidCredentials_ShowsProductList()
    {
        var inventory = await _loginPage.LoginAsync(TestUsers.Standard, TestUsers.Password);

        await Expect(inventory.Header).ToHaveTextAsync("Products");
        Assert.That(await inventory.CountProductsAsync(), Is.EqualTo(6));
    }

    [Test]
    public async Task Login_WithLockedOutUser_ShowsError()
    {
        await _loginPage.SubmitCredentialsAsync(TestUsers.LockedOut, TestUsers.Password);

        await Expect(_loginPage.ErrorMessage).ToContainTextAsync("locked out");
        await Expect(Page).ToHaveURLAsync(TestConfig.BaseUrl);
    }

    [Test]
    public async Task AddProductToCart_ShowsBadgeWithOneItem()
    {
        var inventory = await _loginPage.LoginAsync(TestUsers.Standard, TestUsers.Password);

        await inventory.AddToCartAsync("sauce-labs-backpack");

        await Expect(inventory.CartBadge).ToHaveTextAsync("1");
    }
}
