using MedorPracticalTest.BitcoinPriceService.Abstractions.Services;
using MedorPracticalTest.Domain.Entities.Bitcoin;
using System.Text.Json;

namespace MedorPracticalTest.BitcoinPriceService.Services
{
        internal class BitcoinPriceService : IBitcoinPriceService
        {
                private readonly HttpClient _httpClient;

                public BitcoinPriceService()
                {
                        _httpClient = new HttpClient();
                }

                public async Task<Bitcoin> GetCurrentBitcoinPriceAsync()
                {
                        var apiUrl = "https://api.coindesk.com/v1/bpi/currentprice.json";

                        var response = await _httpClient.GetAsync(apiUrl);
                        response.EnsureSuccessStatusCode();

                        var content = await response.Content.ReadAsStringAsync();

                        var jsonDocument = JsonDocument.Parse(content);

                        var bitcoinPriceUSD = jsonDocument.RootElement
                                .GetProperty("bpi")
                                .GetProperty("USD")
                                .GetProperty("rate_float")
                                .GetDecimal();

                        var bitcoinPriceEUR = jsonDocument.RootElement
                                .GetProperty("bpi")
                                .GetProperty("EUR")
                                .GetProperty("rate_float")
                                .GetDecimal();

                        var bitcoin = new Bitcoin(
                                id: 1,
                                timestamp: DateTime.UtcNow,
                                bitcoinPriceUSD: bitcoinPriceUSD,
                                bitcoinPriceEUR: bitcoinPriceEUR,
                                note: string.Empty
                        );

                        return bitcoin;
                }

                public async Task<IEnumerable<Bitcoin>> GetHistoricalBitcoinPriceAsync(DateTime startDateTime)
                {
                        var endDateTime = DateTime.UtcNow;
                        var endTimestamp = new DateTimeOffset(endDateTime).ToUnixTimeSeconds();
                        var startTimestamp = new DateTimeOffset(startDateTime).ToUnixTimeSeconds();

                        var bitcoinPrices = new List<Bitcoin>();

                        var apiUrl = $"https://api.coingecko.com/api/v3/coins/bitcoin/market_chart/range?vs_currency=eur&from={startTimestamp}&to={endTimestamp}";

                        var response = await _httpClient.GetAsync(apiUrl);
                        response.EnsureSuccessStatusCode();

                        var content = await response.Content.ReadAsStringAsync();
                        var jsonDocument = JsonDocument.Parse(content);
                        var pricesArray = jsonDocument.RootElement.GetProperty("prices");

                        foreach (var priceEntry in pricesArray.EnumerateArray())
                        {
                                var timestamp = priceEntry[0].GetInt64();
                                var priceEur = priceEntry[1].GetDecimal();

                                var priceDateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;

                                var bitcoin = new Bitcoin(
                                    id: 0,
                                    priceDateTime,
                                    0m,//Todo complete usd putting 
                                    priceEur,
                                    0m,
                                    String.Empty
                                );

                                bitcoinPrices.Add(bitcoin);
                        }

                        return bitcoinPrices;
                }
        }
}
