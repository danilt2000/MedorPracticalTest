using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MedorPracticalTest.Application.Requests.Bitcoins.Commands.UpdateBitcoinNoteRequest
{
        public class UpdateBitcoinNoteRequest : IRequest
        {
                [Required]
                public int Id { get; set; }

                public required string Note { get; set; }
        }
}
