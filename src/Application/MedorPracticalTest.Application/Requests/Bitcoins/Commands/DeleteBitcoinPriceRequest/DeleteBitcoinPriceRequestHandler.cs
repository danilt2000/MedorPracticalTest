using MediatR;
using MedorPracticalTest.Persistence.Abstractions.Repositories;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Commands.DeleteBitcoinPriceRequest
{
        internal class DeleteBitcoinPriceRequestHandler : IRequestHandler<DeleteBitcoinPriceRequest>
        {
                private readonly IBitcoinRepository _repository;

                public DeleteBitcoinPriceRequestHandler(IBitcoinRepository repository)
                {
                        _repository = repository;
                }


                public async Task Handle(DeleteBitcoinPriceRequest request, CancellationToken cancellationToken)
                {
                        await _repository.DeleteBitcoinAsync(request.Id);
                }
        }
}
