using Application.Databases.Relational.Models;
using Domain.Features.Comment.Entities;
using Domain.Features.Intership.Entities;
using Domain.Features.Recruitment.Entities;

namespace Application.Features.Internships.Mappers
{
    public interface IInternshipMapper
    {
        DomainRecruitment DomainRecruitment(Recruitment database);
        DomainIntership DomainIntership(Internship database);
        DomainComment DomainComment(Comment database);
    }
}
