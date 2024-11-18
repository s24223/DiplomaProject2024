using Application.Shared.DTOs.Features.Addresses;
using Application.Shared.DTOs.Features.Characteristics.Responses;
using Domain.Features.Person.Entities;

namespace Application.Shared.DTOs.Features.Persons
{
    public class PersonResponseDto
    {
        //values
        public Guid Id { get; set; }
        public DateOnly Created { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhoneNum { get; set; }
        public string? UrlSegment { get; set; }
        public string? Description { get; set; }
        public bool IsStudent { get; set; }
        public bool IsPublicProfile { get; set; }
        public Guid? AddressId { get; set; }
        public AddressResponseDto? Address { get; set; }
        public IEnumerable<CharCollocationResp> Characteristics { get; set; } = [];


        //Cosntructor
        public PersonResponseDto(DomainPerson domain)
        {
            Id = domain.Id.Value;
            Created = domain.Created;
            Name = domain.Name;
            Surname = domain.Surname;
            BirthDate = domain.BirthDate;
            ContactEmail = domain.ContactEmail;
            ContactPhoneNum = (string?)domain.ContactPhoneNum;
            UrlSegment = domain.UrlSegment;
            Description = domain.Description;
            IsStudent = domain.IsStudent;
            IsPublicProfile = domain.IsPublicProfile;

            if (domain.Address != null)
            {
                Address = new AddressResponseDto(domain.Address);
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
