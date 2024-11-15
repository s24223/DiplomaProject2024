using Application.Shared.DTOs.Features.Characteristics.Requests;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsOffer.UpdateOffer.Request
{
    public class UpdateOfferRequestDto
    {
        [Required]
        public required Guid OfferId { get; set; }
        [Required]
        public required string Name { get; set; } = null!;
        [Required]
        public required string Description { get; set; } = null!;
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public bool? IsNegotiatedSalary { get; set; }
        [Required]
        public required bool IsForStudents { get; set; }
        public IEnumerable<CharacteristicCollocationRequestDto> Characteristics { get; set; } = [];
    }
}
