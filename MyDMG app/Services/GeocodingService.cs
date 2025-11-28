using System.Net.Http;
using System.Text.Json;

namespace MyDMG_app.Services
{
    public class GeocodingService
    {
        private readonly string apiUrl = "https://maps.googleapis.com/maps/api/geocode/json";
        private readonly string apiKey = "TU_API_KEY_AQUI"; // <- Sustituye tu API key

        private readonly HttpClient _http = new HttpClient();

        public async Task<(double lat, double lng)?> GetCoordinatesAsync(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return null;

            string url = $"{apiUrl}?address={Uri.EscapeDataString(address)}&key={apiKey}";

            var response = await _http.GetStringAsync(url);

            using JsonDocument doc = JsonDocument.Parse(response);

            var results = doc.RootElement.GetProperty("results");

            if (results.GetArrayLength() == 0)
                return null;

            var location = results[0]
                .GetProperty("geometry")
                .GetProperty("location");

            double lat = location.GetProperty("lat").GetDouble();
            double lng = location.GetProperty("lng").GetDouble();

            return (lat, lng);
        }
    }
}

