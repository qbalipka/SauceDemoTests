namespace Zadanie1.Tests;

/// <summary>
/// Pojedyncze miejsce na dane konfiguracyjne testów.
/// BASE_URL można nadpisać zmienną środowiskową (np. na środowisku testowym / CI).
/// </summary>
public static class TestConfig
{
    public static string BaseUrl =>
        Environment.GetEnvironmentVariable("BASE_URL") ?? "https://www.saucedemo.com/";
}

/// <summary>
/// Konta testowe wypisane wprost na stronie logowania saucedemo.com -
/// publiczne dane demo, nie sekrety.
/// </summary>
public static class TestUsers
{
    public const string Standard = "standard_user";
    public const string LockedOut = "locked_out_user";
    public const string Password = "secret_sauce";
}
