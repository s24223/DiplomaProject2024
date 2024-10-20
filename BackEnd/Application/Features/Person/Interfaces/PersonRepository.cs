using Application.Databases.Relational;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Person.Entities;
using Domain.Features.Person.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Person.Interfaces
{
    public class PersonRepository : IPersonRepository
    {
        //Values
        private readonly IEntityToDomainMapper _mapper;
        private readonly IExceptionsRepository _exceptionRepository;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public PersonRepository
            (
            IEntityToDomainMapper mapper,
            IExceptionsRepository exceptionRepository,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
            _exceptionRepository = exceptionRepository;
            _context = context;
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Public Methods

        //DML
        public async Task CreateAsync
            (
            DomainPerson person,
            CancellationToken cancellation
            )
        {
            try
            {
                var inputDatabasePerson = new Databases.Relational.Models.Person
                {
                    UserId = person.Id.Value,
                    UrlSegment = person.UrlSegment == null ? null : person.UrlSegment.Value,
                    ContactEmail = person.ContactEmail.Value,
                    Name = person.Name,
                    Surname = person.Surname,
                    BirthDate = person.BirthDate == null ? null : person.BirthDate.Value,
                    ContactPhoneNum = person.ContactPhoneNum == null ? null : person.ContactPhoneNum.Value,
                    Description = person.Description,
                    IsStudent = person.IsStudent.Code,
                    IsPublicProfile = person.IsPublicProfile.Code,
                    AddressId = person.AddressId == null ? null : person.AddressId.Value,
                };
                await _context.People.AddAsync(inputDatabasePerson, cancellation);
                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        public async Task UpdateAsync(DomainPerson person, CancellationToken cancellation)
        {
            try
            {
                var databsePerson = await GetDtabasePersonAsync(person.Id, cancellation);

                databsePerson.UrlSegment = person.UrlSegment == null ? null : person.UrlSegment.Value;
                databsePerson.ContactEmail = person.ContactEmail.Value;
                databsePerson.Name = person.Name;
                databsePerson.Surname = person.Surname;
                databsePerson.BirthDate = person.BirthDate == null ? null : person.BirthDate.Value;
                databsePerson.ContactPhoneNum = person.ContactPhoneNum == null ? null : person.ContactPhoneNum.Value;
                databsePerson.Description = person.Description;
                databsePerson.IsStudent = person.IsStudent.Code;
                databsePerson.IsPublicProfile = person.IsPublicProfile.Code;
                databsePerson.AddressId = person.AddressId == null ? null : person.AddressId.Value;

                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        //DQL
        public async Task<DomainPerson> GetPersonAsync(UserId id, CancellationToken cancellation)
        {
            var databasePerson = await GetDtabasePersonAsync(id, cancellation);
            return _mapper.ToDomainPerson(databasePerson);
        }

        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Pivate Methods
        private async Task<Databases.Relational.Models.Person> GetDtabasePersonAsync
            (
            UserId id,
            CancellationToken cancellation
            )
        {
            var databasePerson = await _context.People
                .Where(x => x.UserId == id.Value)
                .FirstOrDefaultAsync(cancellation);

            if (databasePerson == null)
            {
                throw new PersonException
                    (
                    Messages.Person_Ids_NotFound,
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return databasePerson;
        }
    }
}
