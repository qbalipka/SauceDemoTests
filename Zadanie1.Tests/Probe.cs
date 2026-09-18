using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Zadanie1.Tests;

/// <summary>
/// Narzędzie diagnostyczne do grzebania w DOM - nie jest częścią suity testowej.
/// [Explicit] sprawia, że zwykłe "dotnet test" go pomija; uruchamiasz go ręcznie
/// z Ridera albo przez --filter DumpDom.
/// </summary>
[TestFixture]
[Explicit("Narzędzie diagnostyczne, uruchamiane ręcznie")]
public class Probe : PageTest
{
    [Test]
    public async Task DumpDom()
    {
        await Page.GotoAsync(TestConfig.BaseUrl);
        // Celowo DOMContentLoaded, nie NetworkIdle - serwisy z ruchem w tle
        // (analityka, lazy-loading) nigdy nie osiągają stanu bezczynności sieci.
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        Console.WriteLine(await Page.TitleAsync());

        var dump = await Page.EvaluateAsync<string>(@"() => {
            const out = [];
            out.push('TITLE: ' + document.title);
            out.push('URL: ' + location.href);

            out.push('--- inputs / selecty ---');
            document.querySelectorAll('input, textarea, select').forEach(e => {
                out.push('  ' + e.tagName + ' id=' + e.id + ' name=' + (e.name||'')
                    + ' type=' + (e.type||'') + ' ph=' + (e.placeholder||'')
                    + ' dt=' + (e.getAttribute('data-test')||''));
            });

            out.push('--- buttons / linki ---');
            document.querySelectorAll('button, [type=submit], a.btn, .btn').forEach(e => {
                out.push('  ' + e.tagName + ' id=' + e.id
                    + ' dt=' + (e.getAttribute('data-test')||'')
                    + ' :: ' + (e.innerText||'').trim().slice(0,40));
            });

            out.push('--- elementy z data-test ---');
            const seen = new Set();
            document.querySelectorAll('[data-test]').forEach(e => {
                const dt = e.getAttribute('data-test');
                if (seen.has(dt)) return;
                seen.add(dt);
                out.push('  ' + e.tagName + ' [' + dt + '] :: '
                    + (e.innerText||'').trim().slice(0,60).replace(/\n/g, ' | '));
            });

            return out.join('\n');
        }");

        // Katalog tymczasowy systemu - nie wygasa razem z sesją narzędzia.
        var outputPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "probe-dump.txt");
        await File.WriteAllTextAsync(outputPath, dump);

        Console.WriteLine(dump);
        Console.WriteLine($"\nZrzut zapisany do: {outputPath}");
    }
}
