using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Services.API.TemperatureReadings;
using Services.API.TemperatureReadings.Models;

namespace HeatWave.Pages
{
    class ReadingsFiltered
    {
        public List<string> labels { get; set; }
        public List<string> data { get; set; }

        public ReadingsFiltered()
        {
            labels = new List<string>();
            data = new List<string>();
        }
    }

    public class HistoryReadingsModel : PageModel
    {
        private readonly ILogger<HistoryReadingsModel> logger;
        private readonly ITemperatureReadings readings;
        public readonly IConfiguration config;

        public HistoryReadingsModel(IConfiguration config, ILogger<HistoryReadingsModel> logger, ITemperatureReadings readings)
        {
            this.config = config;
            this.logger = logger;
            this.readings = readings;
        }

        public async Task<JsonResult> OnGet()
        {
            ReadingsFiltered readingsFiltered = new ReadingsFiltered();

            // Set timezone setting if provided in appsetting.json.
            string? timezone = this.config.GetValue<string>("Localization:Timezone");

            List<Reading> listReadings = await readings.GetReadings();
            foreach (var item in listReadings)
            {
                DateTime time = item.timestamp;
                if (timezone != null)
                {
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timezone);
                    time = TimeZoneInfo.ConvertTimeFromUtc(item.timestamp, tzi);
                }

                readingsFiltered.labels.Add(time.ToString("HH:mm:ss dd.MM.yyyy"));
                readingsFiltered.data.Add(item.temperature!);
            }

            return new JsonResult(JsonConvert.SerializeObject(readingsFiltered));
        }
    }
}
