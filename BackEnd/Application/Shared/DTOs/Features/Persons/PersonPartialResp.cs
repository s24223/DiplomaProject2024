using Application.Shared.DTOs.Features.Addresses;
using Application.Shared.DTOs.Features.Characteristics.Responses;
using Domain.Features.Person.Entities;

namespace Application.Shared.DTOs.Features.Persons
{
    public class PersonPartialResp
    {
        //values
        public Guid Id { get; set; }
        public DateOnly Created { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int? Age { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhoneNum { get; set; }
        public string? UrlSegment { get; set; }
        public string? Description { get; set; }
        public bool IsStudent { get; set; }
        public bool IsPublicProfile { get; set; }
        public IEnumerable<DivisionResponseDto>? PartialAddress { get; private set; } = null;
        public IEnumerable<CharCollocationResp> Characteristics { get; set; } = [];


        //Constructor
        public PersonPartialResp(DomainPerson domain)
        {
            Id = domain.Id.Value;
            Created = domain.Created;
            Name = domain.Name;
            Surname = domain.Surname;
            Age = domain.Age;
            ContactEmail = domain.ContactEmail;
            ContactPhoneNum = (string?)domain.ContactPhoneNum;
            UrlSegment = domain.UrlSegment;
            Description = domain.Description;
            IsStudent = domain.IsStudent;
            IsPublicProfile = domain.IsPublicProfile;

            if (domain.Address != null)
            {
                PartialAddress = domain.Address.Hierarchy.Select(x => new DivisionResponseDto
                {
                    Id = x.Id.Value,
                    Name = x.Name,
                    ParentId = x.ParentDivisionId,
                    AdministrativeType = new AdministrativeTypeResponseDto
                    {
                        Id = x.DivisionType.Id,
                        Name = x.DivisionType.Name,
                    }
                }).ToList();
            }

            Characteristics = domain.Characteristics.Select(x =>
                new CharCollocationResp(x.Value.Item1, x.Value.Item2)
                );
        }


        //================================================================================================
        //================================================================================================
        //================================================================================================
    }
}
