using Domain.Features.Intership.Entities;

namespace Application.Shared.DTOs.Features.Internships
{
    public class InternshipWithDetailsResp
    {
        //Values
        public InternshipResp Internship { get; init; } = null!;
        public InternshipDetailsResp Details { get; init; } = null!;


        //Constructor
        public InternshipWithDetailsResp(
            DomainIntership domain,
            InternshipDetailsResp details)
        {
            Internship = new InternshipResp(domain);
            Details = details;
        }
    }
}
