namespace Application.VerticalSlice.RecrutmentPart.DTOs.Create
{
    public class CreateRecruitmentRequestDto
    {
        public required Guid BranchId { get; set; }
        public required Guid OfferId { get; set; }
        public required DateTime Created { get; set; }
        public string? PersonMessage { get; set; }
    }
}
