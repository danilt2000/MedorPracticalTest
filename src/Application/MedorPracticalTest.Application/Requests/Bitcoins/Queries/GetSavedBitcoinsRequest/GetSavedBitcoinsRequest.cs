using MediatR;
using MedorPracticalTest.Domain.Entities.Bitcoin;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Queries.GetSavedBitcoinsRequest
{
        public class GetSavedBitcoinsRequest : IRequest<IEnumerable<Bitcoin>>
        {
        }
}
