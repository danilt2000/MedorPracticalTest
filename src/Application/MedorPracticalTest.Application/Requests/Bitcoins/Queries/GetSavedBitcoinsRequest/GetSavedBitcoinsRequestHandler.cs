using MediatR;
using MedorPracticalTest.Domain.Entities.Bitcoin;
using MedorPracticalTest.Persistence.Abstractions.Repositories;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Queries.GetSavedBitcoinsRequest
{
        internal class GetSavedBitcoinsRequestHandler : IRequestHandler<GetSavedBitcoinsRequest, IEnumerable<Bitcoin>>
        {
                private readonly IBitcoinRepository _repository;

                public GetSavedBitcoinsRequestHandler(IBitcoinRepository repository)
                {
                        _repository = repository;
                }

                public async Task<IEnumerable<Bitcoin>> Handle(GetSavedBitcoinsRequest request, CancellationToken cancellationToken)
                {
                        var bitcoins = await _repository.GetBitcoinsAsync();

                        return bitcoins;
                }
        }
}
