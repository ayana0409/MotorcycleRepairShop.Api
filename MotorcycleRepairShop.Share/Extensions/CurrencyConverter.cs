using Newtonsoft.Json.Linq;

namespace MotorcycleRepairShop.Share.Extensions
{
    public static class CurrencyConverter
    {
        private static readonly HttpClient _httpClient = new();
        private static readonly string _apiUrl = "https://www.floatrates.com/daily/vnd.json";

        public static async Task<decimal> ConvertVndToUsd(decimal amountInVnd)
        {
            var response = await _httpClient.GetAsync(_apiUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            var rate = json["usd"]["rate"].Value<decimal>();

            return amountInVnd * rate;
        }

        public static async Task<decimal> ConvertUsdToVnd(decimal amountInUsd)
        {
            var response = await _httpClient.GetAsync(_apiUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            var rate = json["usd"]["rate"].Value<decimal>();

            return amountInUsd / rate;
        }
    }
}
