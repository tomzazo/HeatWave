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
        public HistoryReadingsModel(ILogger<HistoryReadingsModel> logger, ITemperatureReadings readings)
        {
            this.logger = logger;
            this.readings = readings;
        }

        public async Task<JsonResult> OnGet()
        {
            ReadingsFiltered readingsFiltered = new ReadingsFiltered();

            CultureInfo loc = new CultureInfo("sl-SI");
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("CET");


            List<Reading> listReadings = await readings.GetReadings();
            foreach (var item in listReadings)
            {
                readingsFiltered.labels.Add(TimeZoneInfo.ConvertTimeFromUtc(item.timestamp, tzi).ToString(loc));
                readingsFiltered.data.Add(item.temperature!);
            }

            return new JsonResult(JsonConvert.SerializeObject(readingsFiltered));
        }
    }
}
