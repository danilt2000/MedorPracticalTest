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

                //public async Task<Dictionary<DateTime, decimal>> GetEurCzkExchangeRatesForWeekAsync()
                //{
                //        // URL to fetch weekly exchange rates
                //        string apiUrl = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/week.txt";

                //        var response = await _httpClient.GetAsync(apiUrl);
                //        response.EnsureSuccessStatusCode();

                //        var content = await response.Content.ReadAsStringAsync();

                //        string[] lines = content.Split('\n');

                //        var exchangeRates = new Dictionary<DateTime, decimal>();

                //        // Parse each line to find the EUR exchange rates
                //        foreach (var line in lines)
                //        {
                //                if (line.StartsWith("#") || line.StartsWith("Country")) continue; // Skip header lines

                //                string[] parts = line.Split('|');

                //                // Validate line format
                //                if (parts.Length >= 5 && parts[3] == "EUR")
                //                {
                //                        if (DateTime.TryParseExact(parts[0], "dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) &&
                //                            decimal.TryParse(parts[4], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rate))
                //                        {
                //                                exchangeRates[date] = rate;
                //                        }
                //                }
                //        }

                //        return exchangeRates;
                //}


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
        }
}
