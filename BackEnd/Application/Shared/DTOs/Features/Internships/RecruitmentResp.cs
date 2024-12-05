using Domain.Features.Recruitment.Entities;

namespace Application.Shared.DTOs.Features.Internships
{
    public class RecruitmentResp
    {
        //Values
        public Guid PersonId { get; set; }
        public Guid BranchOfferId { get; set; }
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        //public string? CvUrl { get; set; }
        public string? PersonMessage { get; set; }
        public string? CompanyResponse { get; set; }
        public bool? IsAccepted { get; set; }


        //Constructor
        public RecruitmentResp(DomainRecruitment domain)
        {
            PersonId = domain.PersonId.Value;
            BranchOfferId = domain.BranchOfferId.Value;
            Id = domain.Id.Value;
            Created = domain.Created;
            PersonMessage = domain.PersonMessage;
            CompanyResponse = domain.CompanyResponse;
            IsAccepted = domain.IsAccepted?.Value;
        }
    }
}
