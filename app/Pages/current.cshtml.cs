using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.API.TemperatureReadings;
using Services.API.TemperatureReadings.Models;

namespace HeatWave.Pages;

[Authorize]
public class CurrentModel : PageModel
{
    private readonly ILogger<CurrentModel> logger;
    private readonly ITemperatureReadings readings;
    public readonly IConfiguration config;

    public string? Temperature;
    public string? Timestamp;
    public string Emoji = "😎";

    public CurrentModel(IConfiguration config, ILogger<CurrentModel> logger, ITemperatureReadings readings)
    {
        this.config = config;
        this.logger = logger;
        this.readings = readings;
    }

    public async Task OnGet()
    {
        Reading reading = await this.readings.GetReading();

        double temp = Convert.ToDouble(reading.temperature);
        if (temp < 20) Emoji = "🥶";
        else if (temp > 25) Emoji = "🥵";

        Temperature = reading.temperature!;
        DateTime time = reading.timestamp;

        // Set timezone setting if provided in appsetting.json.
        string? timezone = this.config.GetValue<string>("Localization:Timezone");
        if (timezone != null)
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            time = TimeZoneInfo.ConvertTimeFromUtc(reading.timestamp, tzi);
        }

        Timestamp = time.ToString("HH:mm:ss dd.MM.yyyy");
    }
}
