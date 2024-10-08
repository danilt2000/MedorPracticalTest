using MediatR;
using MedorPracticalTest.Domain.Entities.Bitcoin;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Queries.GetHistoricalBitcoinPriceRequest
{
        public class GetHistoricalBitcoinPriceRequest : IRequest<IEnumerable<Bitcoin>>
        {
                public DateTime StartDateTime { get; set; }
        }
}
