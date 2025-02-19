﻿using Application.Databases.Relational.Models;
using Domain.Features.Company.Entities;

namespace Application.Shared.DTOs.Features.Companies.Responses
{
    public class CompanyResp
    {
        //Values
        public Guid CompanyId { get; set; }
        public DateOnly Created { get; set; }
        public string? Name { get; set; }
        public string? Regon { get; set; }
        public string? ContactEmail { get; set; }
        public string? UrlSegment { get; set; }
        public string? Description { get; set; }


        //Cosntructor 
        public CompanyResp(DomainCompany domain)
        {
            CompanyId = domain.Id.Value;
            Created = domain.Created;
            Name = domain.Name;
            Regon = domain.Regon;
            ContactEmail = domain.ContactEmail;
            UrlSegment = domain.UrlSegment;
            Description = domain.Description;
        }

        public CompanyResp(Company database)
        {
            CompanyId = database.UserId;
            Created = database.Created;
            Name = database.Name;
            Regon = database.Regon;
            ContactEmail = database.ContactEmail;
            UrlSegment = database.UrlSegment;
            Description = database.Description;
        }
    }
}
