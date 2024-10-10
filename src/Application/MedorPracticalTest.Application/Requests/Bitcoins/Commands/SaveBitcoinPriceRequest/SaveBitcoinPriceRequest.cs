using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Commands.SaveBitcoinPriceRequest
{
        public class SaveBitcoinPriceRequest : IRequest<int>
        {
                [Required]
                public decimal BitcoinPriceUSD { get; set; }

                [Required]
                public decimal BitcoinPriceEUR { get; set; }

                [Required]
                public decimal BitcoinPriceCZK { get; set; }

                [Required]
                public DateTime Timestamp { get; set; }

                public string? Note { get; set; }
        }
}
