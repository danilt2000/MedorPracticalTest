using MediatR;
using MedorPracticalTest.Domain.Entities.Bitcoin;
using MedorPracticalTest.Persistence.Abstractions.Repositories;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Queries.GetSavedBitcoinsRequest
{
        internal class GetSavedBitcoinsRequestHandler(IBitcoinRepository repository)
                : IRequestHandler<GetSavedBitcoinsRequest, IEnumerable<Bitcoin>>
        {
                public async Task<IEnumerable<Bitcoin>> Handle(GetSavedBitcoinsRequest request, CancellationToken cancellationToken)
                {
                        var bitcoins = await repository.GetBitcoinsAsync();

                        return bitcoins;
                }
        }
}
