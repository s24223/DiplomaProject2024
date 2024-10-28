using Application.Shared.DTOs.Features.Companies;
using Application.Shared.DTOs.Features.Users.Urls;
using Domain.Features.Company.Entities;

namespace Application.Features.Companies.DTOs.QueriesCompany.Shared
{
    public class CompanyDetailsQueryResponseDto : CompanyResponseDto
    {
        public IEnumerable<UrlResponseDto> Urls { get; set; } = new List<UrlResponseDto>();

        public CompanyDetailsQueryResponseDto(DomainCompany domain) : base(domain)
        {
            if (domain.User.Urls != null && domain.User.Urls.Any())
            {
                Urls = domain.User.Urls.Select(x => new UrlResponseDto(x.Value));
            }
        }
    }
}
