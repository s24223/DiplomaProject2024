using Application.Shared.DTOs;

namespace Application.Features.Internships.Commands.Internships.DTOs
{
    public class CreateInternshipReq
    {
        public string ContactNumber { get; set; } = null!;
        public DateOnlyRequestDto ContractStartDate { get; set; } = null!;
        public DateOnlyRequestDto? ContractEndDate { get; set; } = null!;
    }
}
