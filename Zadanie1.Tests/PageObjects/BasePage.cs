using Microsoft.Playwright;

namespace Zadanie1.Tests.PageObjects;

/// <summary>
/// Wspólna baza dla wszystkich Page Objectów: dostęp do strony, budowa adresu
/// i nawigacja.
/// </summary>
public abstract class BasePage
{
    protected readonly IPage Page;

    protected BasePage(IPage page) => Page = page;

    /// <summary>Ścieżka względna wobec <see cref="TestConfig.BaseUrl"/>, np. "/products".</summary>
    protected abstract string Path { get; }

    /// <summary>TrimEnd chroni przed podwójnym ukośnikiem, gdy BaseUrl kończy się "/".</summary>
    public string Url => $"{TestConfig.BaseUrl.TrimEnd('/')}{Path}";

    public virtual Task OpenAsync() => Page.GotoAsync(Url);
}
