using MediatR;
using MedorPracticalTest.BitcoinPriceService.Abstractions.Services;
using MedorPracticalTest.Domain.Entities.Bitcoin;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Queries.GetCurrentBitcoinPriceRequest
{
        public class GetCurrentBitcoinPriceRequestHandler : IRequestHandler<GetCurrentBitcoinPriceRequest, Bitcoin>
        {
                private readonly IBitcoinPriceService _bitcoinPriceService;

                public GetCurrentBitcoinPriceRequestHandler(IBitcoinPriceService bitcoinPriceService)
                {
                        _bitcoinPriceService = bitcoinPriceService;
                }

                public async Task<Bitcoin> Handle(GetCurrentBitcoinPriceRequest request, CancellationToken cancellationToken)
                {
                        var bitcoin = await _bitcoinPriceService.GetCurrentBitcoinPriceAsync();

                        if (bitcoin == null)
                        {
                                //throw new ProductNotFoundException(request.Id);//Todo make custom exception 
                        }

                        return bitcoin;
                }
        }
}
