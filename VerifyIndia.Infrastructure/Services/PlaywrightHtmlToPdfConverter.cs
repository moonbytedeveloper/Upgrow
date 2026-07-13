using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Verification.Documents;

namespace VerifyIndia.Infrastructure.Services
{
    public sealed class PlaywrightHtmlToPdfConverter
        : IHtmlToPdfConverter
    {
        public async Task<byte[]> ConvertAsync(
            string html,
            CancellationToken cancellationToken)
        {
            using var playwright =
                await Playwright.CreateAsync();

            await using var browser =
                await playwright.Chromium
                    .LaunchAsync(
                        new BrowserTypeLaunchOptions
                        {
                            Headless = true
                        });

            var page =
                await browser.NewPageAsync();

            await page.SetContentAsync(
                html,
                new PageSetContentOptions
                {
                    WaitUntil =
                        WaitUntilState.NetworkIdle
                });

            return await page.PdfAsync(
                new PagePdfOptions
                {
                    Format = "A4",
                    PrintBackground = true,
                    Margin = new Margin
                    {
                        Top = "15mm",
                        Bottom = "15mm",
                        Left = "12mm",
                        Right = "12mm"
                    }
                });
        }
    }
}
