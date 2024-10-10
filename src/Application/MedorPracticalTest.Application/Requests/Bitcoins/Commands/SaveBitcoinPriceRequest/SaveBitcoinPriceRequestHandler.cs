using MediatR;
using MedorPracticalTest.Domain.Entities.Bitcoin;
using MedorPracticalTest.Persistence.Abstractions.Repositories;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Commands.SaveBitcoinPriceRequest
{
        public class SaveBitcoinPriceRequestHandler(IBitcoinRepository repository)
                : IRequestHandler<SaveBitcoinPriceRequest, int>
        {
                public async Task<int> Handle(SaveBitcoinPriceRequest request, CancellationToken cancellationToken)
                {
                        var bitcoin = new Bitcoin(
                                id: 0,
                                timestamp: request.Timestamp,
                                bitcoinPriceUSD: request.BitcoinPriceUSD,
                                bitcoinPriceEUR: request.BitcoinPriceEUR,
                                bitcoinPriceCZK: request.BitcoinPriceCZK,
                                note: request.Note
                        );

                        var idBitcoin = await repository.SaveBitcoinAsync(bitcoin);

                        return idBitcoin;
                }
        }
}
