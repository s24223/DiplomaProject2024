namespace Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsOffer.CreateOffer
{
    public class CreateOfferRequestDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public bool? IsNegotiatedSalary { get; set; }
        public bool IsForStudents { get; set; }
    }
}
