using MediatR;
using MedorPracticalTest.Persistence.Abstractions.Repositories;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Commands.UpdateBitcoinNoteRequest
{
        internal class UpdateBitcoinNoteRequestHandler : IRequestHandler<UpdateBitcoinNoteRequest>
        {
                private readonly IBitcoinRepository _repository;

                public UpdateBitcoinNoteRequestHandler(IBitcoinRepository repository)
                {
                        _repository = repository;
                }

                public async Task Handle(UpdateBitcoinNoteRequest request, CancellationToken cancellationToken)
                {
                        await _repository.UpdateBitcoinNoteAsync(request.Id, request.Note);
                }
        }
}
