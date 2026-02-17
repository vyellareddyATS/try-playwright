using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using var playwright = await Playwright.CreateAsync();

await using var browser = await playwright.Chromium.LaunchAsync(new()
{
Headless = true
});

var context = await browser.NewContextAsync();
var page = await context.NewPageAsync();
try
{
await page.GotoAsync(
      "https://inline.app/booking/-Lamo24uNMzLIlnCEhIJ:inline-live-2a466/-Lamo28zt1ere32YxWMR?language=en",
      new() { WaitUntil = WaitUntilState.DOMContentLoaded });
try{
await page.Locator("button.sc-hHLeRK").ClickAsync();
}
catch (Exception er)
{ 
    Console.WriteLine("❌ Click Failed Message : " + er.Message);
    Console.WriteLine("❌ Click Failed StackTrace: " + er.StackTrace);
}
await page.SelectOptionAsync(BookingPage.AdultPicker, "3");

await page.Locator(BookingPage.DatePicker).ClickAsync();

await SelectDateandTime(page, "2026-02-23", "12:00");

await page.Locator(BookingPage.CompleteBookingBtn).ClickAsync();

await page.Locator(BookingPage.ConfirmRuleBtn).ClickAsync();

await page.ScreenshotAsync(new() { Path = "BookNow.png", FullPage = true });

await browser.CloseAsync();
}
catch (Exception ex)
{
    Console.WriteLine("❌ Message: " + ex.Message);
    Console.WriteLine("❌ StackTrace: " + ex.StackTrace);
}
async Task SelectDateandTime(IPage page, string date, string time)
{
    await page.Locator(BookingPage.DateCell(date)).ClickAsync();

    var slots = page.Locator(BookingPage.TimeSlots);
    await slots.First.WaitForAsync();

    var target = slots.Filter(new() { HasTextString = time }).First;

    if (await target.CountAsync() > 0)
        await target.ClickAsync();
    else
        throw new Exception($"Time slot '{time}' not found.");
}

static class BookingPage
{
    public const string AdultPicker = "#adult-picker";
    public const string DatePicker = "div#date-picker";
    public const string CompleteBookingBtn = "button[data-cy='book-now-action-button']";
    public const string ConfirmRuleBtn = "button[data-cy='confirm-house-rule']";
    public const string TimeSlots = "button[data-cy^='book-now-time-slot-box']";

    public static string DateCell(string date) =>
        $"div[data-cy='bt-cal-day'][data-date='{date}']";
}