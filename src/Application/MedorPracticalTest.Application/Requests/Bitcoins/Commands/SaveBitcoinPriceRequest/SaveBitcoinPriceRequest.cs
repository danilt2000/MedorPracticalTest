using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Commands.SaveBitcoinPriceRequest
{
        public class SaveBitcoinPriceRequest : IRequest
        {
                [Required]
                public decimal BitcoinPriceUSD { get; private set; }

                [Required]
                public decimal BitcoinPriceEUR { get; private set; }

                [Required]
                public decimal BitcoinPriceCZK { get; private set; }

                public string? Note { get; private set; }
        }
}
