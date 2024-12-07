﻿using Application.Shared.DTOs.Features.Companies.Responses;
using Application.Shared.DTOs.Features.Users.Urls;
using Domain.Features.Company.Entities;

namespace Application.Features.Companies.Queries.QueriesOffer.DTOs.Shared
{
    public class CompanyDetailsResponseDto : CompanyResp
    {
        //Values
        public IEnumerable<UrlResp> Urls { get; set; } = [];


        //Constructor
        public CompanyDetailsResponseDto(DomainCompany domain) : base(domain)
        {
            if (domain.User.Urls != null && domain.User.Urls.Any())
            {
                Urls = domain.User.Urls.Select(x => new UrlResp(x.Value));
            }
        }
    }
}
