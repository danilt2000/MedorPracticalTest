using MediatR;
using MedorPracticalTest.Domain.Entities.Bitcoin;
using MedorPracticalTest.Persistence.Abstractions.Repositories;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Commands.SaveBitcoinPriceRequest
{
        public class SaveBitcoinPriceRequestHandler : IRequestHandler<SaveBitcoinPriceRequest>
        {
                private readonly IBitcoinRepository _repository;

                public SaveBitcoinPriceRequestHandler(IBitcoinRepository repository)
                {
                        _repository = repository;
                }

                public async Task Handle(SaveBitcoinPriceRequest request, CancellationToken cancellationToken)
                {
                        var bitcoin = new Bitcoin(
                                id: 0,
                                timestamp: request.Timestamp,
                                bitcoinPriceUSD: request.BitcoinPriceUSD,
                                bitcoinPriceEUR: request.BitcoinPriceEUR,
                                bitcoinPriceCZK: request.BitcoinPriceCZK,
                                note: request.Note
                        );

                        await _repository.SaveBitcoinDataAsync(bitcoin);
                }
        }
}
