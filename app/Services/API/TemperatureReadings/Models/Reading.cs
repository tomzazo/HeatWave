namespace Services.API.TemperatureReadings.Models
{
    public class Reading
    {
        public Guid id { get; set; }
        public string? paritionKey { get; set; }
        public string? temperature { get; set; }
        public DateTime timestamp { get; set; }
    }
}