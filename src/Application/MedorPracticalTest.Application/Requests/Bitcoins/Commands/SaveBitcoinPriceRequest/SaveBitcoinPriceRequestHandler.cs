using MediatR;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Commands.SaveBitcoinPriceRequest
{
        public class SaveBitcoinPriceRequestHandler : IRequestHandler<SaveBitcoinPriceRequest>
        {
                //private readonly IProductRepository _repository;

                public SaveBitcoinPriceRequestHandler(/*IProductRepository repository*/)
                {
                        //_repository = repository;
                }

                public Task Handle(SaveBitcoinPriceRequest request, CancellationToken cancellationToken)
                {
                        throw new NotImplementedException();
                }
        }
}
