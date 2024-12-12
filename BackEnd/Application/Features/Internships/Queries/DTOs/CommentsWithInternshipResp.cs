using Application.Shared.DTOs.Features.Companies.Responses;
using Application.Shared.DTOs.Features.Internships;
using Application.Shared.DTOs.Features.Internships.Comments;
using Application.Shared.DTOs.Features.Persons;
using Domain.Features.Intership.Entities;

namespace Application.Features.Internships.Queries.DTOs
{
    public class CommentsWithInternshipResp
    {
        //Values
        public PersonPartialResp Person { get; set; } = null!;
        public CompanyResp Company { get; set; } = null!;
        public BranchResp Branch { get; set; } = null!;
        public BranchOfferResp BranchOffer { get; set; } = null!;
        public OfferResp Offer { get; set; } = null!;
        public InternshipResp Internship { get; init; } = null!;
        public InternshipDetailsResp Details { get; init; } = null!;
        public IEnumerable<CommentResp> Comments { get; init; } = [];
        public int TotalCount { get; init; } = 0;


        //Constructor
        public CommentsWithInternshipResp(
            DomainIntership domain,
            InternshipDetailsResp details,
            int totalCount)
        {
            Internship = new InternshipResp(domain);
            Details = details;
            TotalCount = totalCount;
            Comments = domain.Comments.Values.Select(c => new CommentResp(c));

            if (domain.Recrutment.BranchOffer != null)
            {
                BranchOffer = new BranchOfferResp(domain.Recrutment.BranchOffer);
            }
            if (domain.Recrutment.BranchOffer?.Offer != null)
            {
                Offer = new OfferResp(domain.Recrutment.BranchOffer.Offer);
            }
            if (domain.Recrutment.BranchOffer?.Branch != null)
            {
                Branch = new BranchResp(domain.Recrutment.BranchOffer.Branch);
            }
            if (domain.Recrutment.BranchOffer?.Branch?.Company != null)
            {
                Company = new CompanyResp(domain.Recrutment.BranchOffer.Branch.Company);
            }
            if (domain.Recrutment.Person != null)
            {
                Person = new PersonPartialResp(domain.Recrutment.Person);
            }
        }
    }
}
