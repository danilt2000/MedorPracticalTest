using MediatR;
using MedorPracticalTest.Domain.Entities.Bitcoin;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Queries.GetCurrentBitcoinPriceRequest
{
        public class GetCurrentBitcoinPriceRequest : IRequest<Bitcoin>
        {
        }
}
