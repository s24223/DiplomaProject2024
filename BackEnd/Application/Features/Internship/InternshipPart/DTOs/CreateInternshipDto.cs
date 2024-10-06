namespace Application.Features.Internship.InternshipPart.DTOs
{
    public class CreateInternshipDto
    {
        public Guid BranchId { get; set; }
        public Guid OfferId { get; set; }
        public string ContactNumber { get; set; } = null!;
        public DateTime Created { get; } = DateTime.Now;
    }
}
