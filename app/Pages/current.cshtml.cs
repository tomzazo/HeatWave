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
    public string? Temperature;
    public string? Timestamp;
    public string Emoji = "😎";

    public CurrentModel(ILogger<CurrentModel> logger, ITemperatureReadings readings)
    {
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
        Timestamp = reading.timestamp.ToString("HH:mm:ss dd.MM.yyyy");
    }
}
