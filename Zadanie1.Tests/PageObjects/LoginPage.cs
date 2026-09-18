using Microsoft.Playwright;

namespace Zadanie1.Tests.PageObjects;

/// <summary>Strona logowania Swag Labs (saucedemo.com).</summary>
public class LoginPage : BasePage
{
    public LoginPage(IPage page) : base(page) { }

    protected override string Path => "/";

    private ILocator UsernameInput => Page.Locator("[data-test=username]");
    private ILocator PasswordInput => Page.Locator("[data-test=password]");
    private ILocator LoginButton => Page.Locator("[data-test=login-button]");

    /// <summary>Komunikat błędu nad formularzem - publiczny, bo asercje robimy w testach.</summary>
    public ILocator ErrorMessage => Page.Locator("[data-test=error]");

    /// <summary>Wypełnia formularz i zatwierdza, bez oczekiwania na wynik.</summary>
    public async Task SubmitCredentialsAsync(string username, string password)
    {
        await UsernameInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();
    }

    /// <summary>Loguje się i zwraca stronę produktów. Dla poprawnych danych.</summary>
    public async Task<InventoryPage> LoginAsync(string username, string password)
    {
        await SubmitCredentialsAsync(username, password);

        var inventory = new InventoryPage(Page);
        await inventory.WaitUntilLoadedAsync();
        return inventory;
    }
}
