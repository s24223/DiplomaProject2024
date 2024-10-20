using Application.Shared.DTOs;

namespace Application.Features.Internship.InternshipPart.DTOs.Update
{
    public class UpdateInternshipRequestDto
    {
        public string ContractNumber { get; set; } = null!;
        public DateOnlyRequestDto ContractStartDate { get; set; } = null!;
        public DateOnlyRequestDto? ContractEndDate { get; set; } = null!;
    }
}
