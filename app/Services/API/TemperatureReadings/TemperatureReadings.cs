using Azure.Core;
using HeatWave;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Services.API.TemperatureReadings.Models;
using Services.Authentication.Azure;

namespace Services.API.TemperatureReadings
{
    public interface ITemperatureReadings
    {
        public Task<Reading> GetReading();
        public Task<List<Reading>> GetReadings();
    }

    public class TemperatureReadings : ITemperatureReadings
    {
        IAzureAuthentication azureAuthentication;

        public TemperatureReadings(IConfiguration config)
        {
            azureAuthentication = new AzureManagedIdentityAuthentication(config);
        }

        public async Task<Reading> GetReading()
        {
            AccessToken accessToken = await this.azureAuthentication.GetAccessTokenAsync();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", accessToken.Token));
            HttpResponseMessage res = await client.GetAsync("https://iot-temp-fn-app.azurewebsites.net/api/latest");
            if (res.StatusCode >= System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(string.Format("Received error {0} on fetching latest temperature from API.", res.StatusCode));
            }

            string? body = await res.Content.ReadAsStringAsync();
            if (body == null)
            {
                throw new Exception("Received null body.");
            }

            return JsonConvert.DeserializeObject<Reading>(body)!;
        }

        public async Task<List<Reading>> GetReadings()
        {
            AccessToken accessToken = await this.azureAuthentication.GetAccessTokenAsync();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", accessToken.Token));
            HttpResponseMessage res = await client.GetAsync("https://iot-temp-fn-app.azurewebsites.net/api/history");
            if (res.StatusCode >= System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(string.Format("Received error {0} on fetching latest temperature from API.", res.StatusCode));
            }

            string? body = await res.Content.ReadAsStringAsync();
            if (body == null)
            {
                throw new Exception("Received null body.");
            }

            return JsonConvert.DeserializeObject<List<Reading>>(body)!;
        }
    }
}