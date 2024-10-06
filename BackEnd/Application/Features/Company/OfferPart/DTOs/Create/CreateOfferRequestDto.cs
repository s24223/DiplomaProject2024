namespace Application.Features.Company.OfferPart.DTOs.Create
{
    public class CreateOfferRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public string? NegotiatedSalary { get; set; }
        public string ForStudents { get; set; } = null!;
    }
}
