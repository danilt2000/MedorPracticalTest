using MediatR;
using MedorPracticalTest.BitcoinPriceService.Abstractions.Services;
using MedorPracticalTest.CnbExchangeService.Abstractions.Services;
using MedorPracticalTest.Domain.Entities.Bitcoin;
using System.Globalization;
using System.Net.Http;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Queries.GetHistoricalBitcoinPriceRequest
{
        public class GetHistoricalBitcoinPriceRequestHandler : IRequestHandler<GetHistoricalBitcoinPriceRequest, IEnumerable<Bitcoin>>
        {
                private readonly IBitcoinPriceService _bitcoinPriceService;

                private readonly ICnbExchangeService _cnbExchangeService;

                public GetHistoricalBitcoinPriceRequestHandler(IBitcoinPriceService bitcoinPriceService,
                        ICnbExchangeService cnbExchangeService)
                {
                        _bitcoinPriceService = bitcoinPriceService;

                        _cnbExchangeService = cnbExchangeService;
                }

                public async Task<IEnumerable<Bitcoin>> Handle(GetHistoricalBitcoinPriceRequest request, CancellationToken cancellationToken)
                {
                        var bitcoins = await
                                _bitcoinPriceService.GetHistoricalBitcoinPriceAsync(request.StartDateTime);

                        bitcoins = await AddCzkPricesToBitcoinListAsync(bitcoins);

                        var enumerable = bitcoins as Bitcoin[] ?? bitcoins.ToArray();

                        if (enumerable.Length < 1)
                        {
                                //throw new ProductNotFoundException(request.Id);//Todo make custom exception 
                        }

                        return enumerable;
                }

                private async Task<List<Bitcoin>> AddCzkPricesToBitcoinListAsync(IEnumerable<Bitcoin> bitcoins)
                {
                        var bitcoinListWithCzkPrices = new List<Bitcoin>();

                        foreach (var bitcoin in bitcoins)
                        {
                                var eurToCzkRate = await _cnbExchangeService.GetEurCzkExchangeRateAsync(bitcoin.Timestamp.Date);//Todo rewrite the whole logic by getting data once from api

                                var bitcoinWithCzkPrice = new Bitcoin(
                                        bitcoin.Id,
                                        bitcoin.Timestamp,
                                        bitcoin.BitcoinPriceUSD,
                                        bitcoin.BitcoinPriceEUR,
                                        bitcoin.BitcoinPriceEUR * eurToCzkRate,
                                        bitcoin.Note
                                );

                                bitcoinListWithCzkPrices.Add(bitcoinWithCzkPrice);
                        }

                        return bitcoinListWithCzkPrices;
                        //var minDate = bitcoins.Min(b => b.Timestamp.Date);
                        //var maxDate = bitcoins.Max(b => b.Timestamp.Date);

                        //// Fetch exchange rates for the entire range (you can limit it to a week or more)
                        //var exchangeRates = await GetEurCzkExchangeRatesForRangeAsync(minDate, maxDate);

                        //var bitcoinListWithCzkPrices = new List<Bitcoin>();

                        //foreach (var bitcoin in bitcoins)
                        //{
                        //        // Lookup the correct exchange rate for the given date
                        //        if (exchangeRates.TryGetValue(bitcoin.Timestamp.Date, out var eurToCzkRate))
                        //        {
                        //                var bitcoinWithCzkPrice = new Bitcoin(
                        //                        bitcoin.Id,
                        //                        bitcoin.Timestamp,
                        //                        bitcoin.BitcoinPriceUSD,
                        //                        bitcoin.BitcoinPriceEUR,
                        //                        bitcoin.BitcoinPriceEUR * eurToCzkRate, // Convert to CZK
                        //                        bitcoin.Note
                        //                );

                        //                bitcoinListWithCzkPrices.Add(bitcoinWithCzkPrice);
                        //        }
                        //        else
                        //        {
                        //                // Handle the case where the rate for a specific date is not found
                        //                throw new Exception($"Exchange rate for EUR to CZK on {bitcoin.Timestamp.Date} not found.");
                        //        }
                        //}

                        //return bitcoinListWithCzkPrices;
                }

                public async Task<Dictionary<DateTime, decimal>> GetEurCzkExchangeRatesForRangeAsync(DateTime startDate, DateTime endDate)
                {
                        //string apiUrl = $"https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt?start={startDate:dd.MM.yyyy}&end={endDate:dd.MM.yyyy}";

                        //var _httpClient = new HttpClient();//Todo delete

                        //var response = await _httpClient.GetAsync(apiUrl);
                        //response.EnsureSuccessStatusCode();

                        //var content = await response.Content.ReadAsStringAsync();

                        //string[] lines = content.Split('\n');

                        //var exchangeRates = new Dictionary<DateTime, decimal>();

                        //foreach (var line in lines)
                        //{
                        //        if (line.StartsWith("#") || line.StartsWith("Country")) continue;

                        //        string[] parts = line.Split('|');

                        //        if (parts.Length >= 5 && parts[3] == "EUR")
                        //        {
                        //                if (DateTime.TryParseExact(parts[0], "dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) &&
                        //                    decimal.TryParse(parts[4], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rate))
                        //                {
                        //                        exchangeRates[date] = rate;
                        //                }
                        //        }
                        //}

                        //return exchangeRates;

                        string apiUrl = $"https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";

                        var _httpClient = new HttpClient();//Todo delete

                        var response = await _httpClient.GetAsync(apiUrl);
                        response.EnsureSuccessStatusCode();

                        var content = await response.Content.ReadAsStringAsync();

                        string[] lines = content.Split('\n');

                        var exchangeRates = new Dictionary<DateTime, decimal>();

                        // Parse each line to find the EUR exchange rates
                        foreach (var line in lines)
                        {
                                if (line.StartsWith("#") || line.StartsWith("Country")) continue; // Skip header lines

                                string[] parts = line.Split('|');

                                // Validate line format
                                if (parts.Length >= 5 && parts[3] == "EUR")
                                {
                                        if (DateTime.TryParseExact(parts[0], "dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) &&
                                            decimal.TryParse(parts[4], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rate))
                                        {
                                                exchangeRates[date] = rate;
                                        }
                                }
                        }

                        return exchangeRates;
                }
        }
}
