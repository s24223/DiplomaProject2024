namespace Application.VerticalSlice.PersonPart.DTOs.Create
{
    public class CreatePersonRequestDto
    {
        public string? UrlSegment { get; set; }
        public required string ContactEmail { get; set; } = null!;
        public required string Name { get; set; } = null!;
        public required string Surname { get; set; } = null!;
        public DateOnly? BirthDate { get; set; }
        public string? ContactPhoneNum { get; set; }
        public string? Description { get; set; }
        public required bool IsStudent { get; set; }
        public required string IsPublicProfile { get; set; } = null!;
        public Guid? AddressId { get; set; }
    }
}
