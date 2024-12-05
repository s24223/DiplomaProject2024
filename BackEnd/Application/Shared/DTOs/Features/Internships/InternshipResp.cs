using Domain.Features.Intership.Entities;

namespace Application.Shared.DTOs.Features.Internships
{
    public class InternshipResp
    {
        //Values
        public Guid Id { get; set; }
        public string ContractNumber { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateOnly ContractStartDate { get; set; }
        public DateOnly? ContractEndDate { get; set; }


        //Constructor
        public InternshipResp(DomainIntership domain)
        {
            Id = domain.Id.Value;
            ContractNumber = domain.ContractNumber.Value;
            Created = domain.Created;
            ContractStartDate = domain.ContractStartDate;
            ContractEndDate = domain.ContractEndDate;
        }
    }
}
