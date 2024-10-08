using MedorPracticalTest.BitcoinPriceService.Abstractions.Services;
using MedorPracticalTest.Domain.Entities.Bitcoin;
using System.Globalization;
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
                        var endTimestamp = new DateTimeOffset(endDateTime).ToUnixTimeMilliseconds();
                        var startTimestamp = new DateTimeOffset(startDateTime).ToUnixTimeMilliseconds();

                        var bitcoinPrices = new List<Bitcoin>();
                        
                        //Todo write comments why binance api was used instead of coindesk
                        var usdApiUrl = $"https://api.binance.com/api/v3/klines?symbol=BTCUSDT&interval=5m&startTime={startTimestamp}&endTime={endTimestamp}";
                        var eurApiUrl = $"https://api.binance.com/api/v3/klines?symbol=BTCEUR&interval=5m&startTime={startTimestamp}&endTime={endTimestamp}";

                        var usdResponse = await _httpClient.GetAsync(usdApiUrl);
                        usdResponse.EnsureSuccessStatusCode();
                        var usdContent = await usdResponse.Content.ReadAsStringAsync();
                        var usdJsonDocument = JsonDocument.Parse(usdContent);

                        var eurResponse = await _httpClient.GetAsync(eurApiUrl);
                        eurResponse.EnsureSuccessStatusCode();
                        var eurContent = await eurResponse.Content.ReadAsStringAsync();
                        var eurJsonDocument = JsonDocument.Parse(eurContent);

                        var usdPricesArray = usdJsonDocument.RootElement.EnumerateArray();
                        var eurPricesArray = eurJsonDocument.RootElement.EnumerateArray();

                        var usdEnumerator = usdPricesArray.GetEnumerator();
                        var eurEnumerator = eurPricesArray.GetEnumerator();

                        while (usdEnumerator.MoveNext() && eurEnumerator.MoveNext())
                        {
                                var usdPriceEntry = usdEnumerator.Current;
                                var eurPriceEntry = eurEnumerator.Current;

                                var timestamp = usdPriceEntry[0].GetInt64();

                                var usdClosePrice = decimal.Parse(usdPriceEntry[4].GetString()!, CultureInfo.InvariantCulture);
                                var eurClosePrice = decimal.Parse(eurPriceEntry[4].GetString()!, CultureInfo.InvariantCulture);

                                var priceDateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;

                                var bitcoin = new Bitcoin(
                                    id: 0,
                                    priceDateTime,
                                    usdClosePrice,
                                    eurClosePrice,
                                    0m,
                                    String.Empty
                                );

                                bitcoinPrices.Add(bitcoin);
                        }

                        usdEnumerator.Dispose();

                        eurEnumerator.Dispose();

                        return bitcoinPrices;
                }
        }
}
