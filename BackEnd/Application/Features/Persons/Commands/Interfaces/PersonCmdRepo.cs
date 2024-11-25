using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Persons.Mappers;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Person.Entities;
using Domain.Features.Person.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Persons.Commands.Interfaces
{
    public class PersonCmdRepo : IPersonCmdRepo
    {
        //Values
        private readonly IPersonMapper _mapper;
        private readonly IExceptionsRepository _exceptionRepository;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public PersonCmdRepo
            (
            IPersonMapper mapper,
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
        public async Task<DomainPerson> CreateAsync
            (
            DomainPerson person,
            CancellationToken cancellation
            )
        {
            try
            {
                var databasePerson = new Person
                {
                    UserId = person.Id.Value,
                    UrlSegment = person.UrlSegment?.Value,
                    ContactEmail = person.ContactEmail.Value,
                    Name = person.Name,
                    Surname = person.Surname,
                    BirthDate = person.BirthDate == null ? null : person.BirthDate.Value,
                    ContactPhoneNum = person.ContactPhoneNum?.Value,
                    Description = person.Description,
                    IsStudent = person.IsStudent.Code,
                    IsPublicProfile = person.IsPublicProfile.Code,
                    AddressId = person.AddressId?.Value,
                };
                var characteristics = person.Characteristics.Select(x => new PersonCharacteristic
                {
                    Person = databasePerson,
                    CharacteristicId = x.Value.Item1.Id.Value,
                    QualityId = x.Value.Item2?.Id.Value,
                });

                await _context.People.AddAsync(databasePerson, cancellation);
                await _context.PersonCharacteristics.AddRangeAsync(characteristics, cancellation);
                await _context.SaveChangesAsync(cancellation);
                return await _mapper.DomainPerson(databasePerson, cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionRepository.ConvertEFDbException(ex);
            }
        }

        public async Task<DomainPerson> UpdateAsync(DomainPerson person, CancellationToken cancellation)
        {
            try
            {
                var databasePerson = await GetDtabasePersonAsync(person.Id, cancellation);

                databasePerson.UrlSegment = person.UrlSegment?.Value;
                databasePerson.ContactEmail = person.ContactEmail.Value;
                databasePerson.Name = person.Name;
                databasePerson.Surname = person.Surname;
                databasePerson.BirthDate = person.BirthDate == null ? null : person.BirthDate.Value;
                databasePerson.ContactPhoneNum = person.ContactPhoneNum?.Value;
                databasePerson.Description = person.Description;
                databasePerson.IsStudent = person.IsStudent.Code;
                databasePerson.IsPublicProfile = person.IsPublicProfile.Code;
                databasePerson.AddressId = person.AddressId?.Value;


                databasePerson.PersonCharacteristics.Clear();
                foreach (var characteristic in person.Characteristics)
                {
                    databasePerson.PersonCharacteristics.Add(new PersonCharacteristic
                    {
                        Person = databasePerson,
                        CharacteristicId = characteristic.Value.Item1.Id.Value,
                        QualityId = characteristic.Value.Item2?.Id.Value,
                    });
                }

                await _context.SaveChangesAsync(cancellation);
                return await _mapper.DomainPerson(databasePerson, cancellation);
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
            return await _mapper.DomainPerson(databasePerson, cancellation);
        }

        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Pivate Methods
        private async Task<Person> GetDtabasePersonAsync
            (
            UserId id,
            CancellationToken cancellation
            )
        {
            var databasePerson = await _context.People
                .Include(x => x.PersonCharacteristics)
                .Where(x => x.UserId == id.Value)
                .FirstOrDefaultAsync(cancellation);

            if (databasePerson == null)
            {
                throw new PersonException
                    (
                    Messages2.Person_Ids_NotFound,
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return databasePerson;
        }

        private async Task
            IsUniqueValuesCreateAsync(DomainPerson domain, CancellationToken cancellation)
        {
            //[ContactEmail] requered
            //[ContactPhoneNum] optional
            //[UrlSegment] optional
            //var query = _context.People.AsQueryable();
            var list = await _context.People.Where(x =>
                (x.ContactEmail != null && x.ContactEmail == domain.ContactEmail) ||
                (x.ContactPhoneNum != null && domain.ContactPhoneNum != null && x.ContactPhoneNum == domain.ContactPhoneNum.Value) ||
                (x.UrlSegment != null && domain.UrlSegment != null && x.UrlSegment == domain.UrlSegment.Value)
                )
                .ToListAsync(cancellation);

        }

        private async Task
            IsUniqueValuesUpdateAsync(DomainPerson domain, CancellationToken cancellation)
        {
            var list = await _context.People.Where(x =>
                (x.ContactEmail != null && x.ContactEmail == domain.ContactEmail) ||
                (x.ContactPhoneNum != null && domain.ContactPhoneNum != null && x.ContactPhoneNum == domain.ContactPhoneNum.Value) ||
                (x.UrlSegment != null && domain.UrlSegment != null && x.UrlSegment == domain.UrlSegment.Value)
                )
                .ToListAsync(cancellation);




        }

        private System.Exception HandleException(System.Exception ex, DomainPerson domain)
        {
            /*
            Person_CHECK_IsStudent
            Person_CHECK_IsPublicProfile
            Person_CHECK_BirthDate
            Person_UNIQUE_ContactEmail
             */


            return ex;
        }
    }
}
