using MedorPracticalTest.CnbExchangeService.Abstractions.Services;
using System.Globalization;

namespace MedorPracticalTest.CnbExchangeService.Services
{
        internal class CnbExchangeService : ICnbExchangeService
        {
                private readonly HttpClient _httpClient;

                public CnbExchangeService()
                {
                        _httpClient = new HttpClient();
                }

                public async Task<decimal> GetEurCzkExchangeRateAsync(DateTime date)
                {
                        string dateStr = date.ToString("dd.MM.yyyy");

                        string apiUrl = $"https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt?date={dateStr}";

                        var response = await _httpClient.GetAsync(apiUrl);
                        response.EnsureSuccessStatusCode();

                        var content = await response.Content.ReadAsStringAsync();

                        string[] lines = content.Split('\n');

                        foreach (var line in lines)
                        {
                                if (line.Contains("EUR"))
                                {
                                        string[] parts = line.Split('|');
                                        if (parts.Length >= 5 && decimal.TryParse(parts[4], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rate))
                                        {
                                                return rate;
                                        }
                                }
                        }

                        throw new Exception($"Exchange rate for EUR to CZK on {dateStr} not found.");
                }

                public async Task<Dictionary<DateTime, decimal>> GetEurCzkExchangeRatesForDatesAsync(IEnumerable<DateTime> dates)
                {
                        var tasks = new List<Task<KeyValuePair<DateTime, decimal>>>();

                        foreach (var date in dates)
                        {
                                tasks.Add(GetExchangeRateForDateAsync(date));
                        }

                        var results = await Task.WhenAll(tasks);

                        return results.ToDictionary(x => x.Key, x => x.Value);
                }

                private async Task<KeyValuePair<DateTime, decimal>> GetExchangeRateForDateAsync(DateTime date)
                {
                        string dateStr = date.ToString("dd.MM.yyyy");
                        string apiUrl = $"https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt?date={dateStr}";

                        var response = await _httpClient.GetAsync(apiUrl);
                        response.EnsureSuccessStatusCode();

                        var content = await response.Content.ReadAsStringAsync();
                        string[] lines = content.Split('\n');

                        foreach (var line in lines)
                        {
                                if (line.Contains("EUR"))
                                {
                                        string[] parts = line.Split('|');
                                        if (parts.Length >= 5 && decimal.TryParse(parts[4], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rate))
                                        {
                                                return new KeyValuePair<DateTime, decimal>(date, rate);
                                        }
                                }
                        }

                        throw new Exception($"Exchange rate for EUR to CZK on {dateStr} not found.");
                }
        }
}
