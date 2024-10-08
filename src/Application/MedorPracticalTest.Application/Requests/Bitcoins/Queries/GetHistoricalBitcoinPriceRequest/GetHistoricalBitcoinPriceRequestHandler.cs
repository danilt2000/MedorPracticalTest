using MediatR;
using MedorPracticalTest.BitcoinPriceService.Abstractions.Services;
using MedorPracticalTest.CnbExchangeService.Abstractions.Services;
using MedorPracticalTest.Domain.Entities.Bitcoin;

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
                        var bitcoins = await _bitcoinPriceService.GetHistoricalBitcoinPriceAsync(request.StartDateTime);

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
                        var enumerableBitcoins = bitcoins as Bitcoin[] ?? bitcoins.ToArray();

                        var uniqueDates = enumerableBitcoins.Select(b => b.Timestamp.Date).Distinct();

                        var exchangeRates = await _cnbExchangeService.GetEurCzkExchangeRatesForDatesAsync(uniqueDates);

                        var bitcoinListWithCzkPrices = new List<Bitcoin>();

                        foreach (var bitcoin in enumerableBitcoins)
                        {
                                if (exchangeRates.TryGetValue(bitcoin.Timestamp.Date, out var eurToCzkRate))
                                {
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
                                else
                                {
                                        throw new Exception($"Exchange rate for {bitcoin.Timestamp.Date} not found.");
                                }
                        }

                        return bitcoinListWithCzkPrices;
                }
        }
}
