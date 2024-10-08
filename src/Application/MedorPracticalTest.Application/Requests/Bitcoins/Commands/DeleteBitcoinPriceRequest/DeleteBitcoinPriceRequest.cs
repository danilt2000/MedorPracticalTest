using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Commands.DeleteBitcoinPriceRequest
{
        public class DeleteBitcoinPriceRequest : IRequest
        {
                [Required]
                public int Id { get; set; }
        }
}
