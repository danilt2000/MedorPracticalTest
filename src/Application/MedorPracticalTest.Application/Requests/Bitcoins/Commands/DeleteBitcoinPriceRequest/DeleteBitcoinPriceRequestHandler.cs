using MediatR;
using MedorPracticalTest.Persistence.Abstractions.Repositories;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Commands.DeleteBitcoinPriceRequest
{
        internal class DeleteBitcoinPriceRequestHandler(IBitcoinRepository repository)
                : IRequestHandler<DeleteBitcoinPriceRequest>
        {
                public async Task Handle(DeleteBitcoinPriceRequest request, CancellationToken cancellationToken)
                {
                        await repository.DeleteBitcoinAsync(request.Id);
                }
        }
}
