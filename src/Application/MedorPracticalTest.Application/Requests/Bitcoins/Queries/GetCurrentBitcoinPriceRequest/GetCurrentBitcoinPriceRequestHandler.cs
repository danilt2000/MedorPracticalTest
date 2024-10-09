using MediatR;
using MedorPracticalTest.Application.Exceptions;
using MedorPracticalTest.BitcoinPriceService.Abstractions.Services;
using MedorPracticalTest.CnbExchangeService.Abstractions.Services;
using MedorPracticalTest.Domain.Entities.Bitcoin;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Queries.GetCurrentBitcoinPriceRequest
{
        public class GetCurrentBitcoinPriceRequestHandler : IRequestHandler<GetCurrentBitcoinPriceRequest, Bitcoin>
        {
                private readonly IBitcoinPriceService _bitcoinPriceService;

                private readonly ICnbExchangeService _cnbExchangeService;

                public GetCurrentBitcoinPriceRequestHandler(IBitcoinPriceService bitcoinPriceService, ICnbExchangeService cnbExchangeService)
                {
                        _bitcoinPriceService = bitcoinPriceService;

                        _cnbExchangeService = cnbExchangeService;
                }

                public async Task<Bitcoin> Handle(GetCurrentBitcoinPriceRequest request, CancellationToken cancellationToken)
                {
                        var tempBitcoin = await _bitcoinPriceService.GetCurrentBitcoinPriceAsync();

                        var eurToCzkRate = await _cnbExchangeService.GetEurCzkExchangeRateAsync(tempBitcoin.Timestamp);

                        if (tempBitcoin == null)
                                throw new BitcoinNotFoundException(tempBitcoin!.Timestamp);

                        var bitcoinPriceInCzk = tempBitcoin.BitcoinPriceEUR * eurToCzkRate;

                        var bitcoin = new Bitcoin(
                                tempBitcoin.Id,
                                tempBitcoin.Timestamp,
                                tempBitcoin.BitcoinPriceUSD,
                                tempBitcoin.BitcoinPriceEUR,
                                bitcoinPriceInCzk,
                                tempBitcoin.Note
                        );

                        return bitcoin;
                }
        }
}
