using Application.Shared.DTOs;

namespace Application.Features.Internship.InternshipPart.DTOs.Create
{
    public class CreateInternshipRequestDto
    {
        public string ContactNumber { get; set; } = null!;
        public DateOnlyRequestDto ContractStartDate { get; set; } = null!;
        public DateOnlyRequestDto? ContractEndDate { get; set; } = null!;
    }
}
