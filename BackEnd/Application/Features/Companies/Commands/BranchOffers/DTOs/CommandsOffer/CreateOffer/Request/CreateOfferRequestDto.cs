using Application.Shared.DTOs.Features.Characteristics.Requests;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsOffer.CreateOffer.Request
{
    public class CreateOfferRequestDto
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public bool? IsNegotiatedSalary { get; set; }
        [Required]
        public bool IsForStudents { get; set; }
        public IEnumerable<CharacteristicCollocationRequestDto> Characteristics { get; set; } = [];
    }
}
