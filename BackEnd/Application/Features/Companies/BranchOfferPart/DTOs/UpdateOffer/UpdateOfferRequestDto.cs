namespace Application.Features.Companies.BranchOfferPart.DTOs.UpdateOffer
{
    public class UpdateOfferRequestDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public bool? IsNegotiatedSalary { get; set; }
        public bool IsForStudents { get; set; }
    }
}
