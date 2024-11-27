using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Persons.Mappers;
using Domain.Features.Person.Entities;
using Domain.Features.Person.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Application.Features.Persons.Commands.Interfaces
{
    public class PersonCmdRepo : IPersonCmdRepo
    {
        //Values
        private readonly IPersonMapper _mapper;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public PersonCmdRepo
            (
            IPersonMapper mapper,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
            _context = context;
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Public Methods

        //DML
        public async Task<DomainPerson> CreatePersonAsync
            (DomainPerson person, CancellationToken cancellation)
        {
            await UniqueValuesHandlerAsync(person, true, cancellation);
            try
            {
                var databasePerson = MapToDbPerson(person);
                var characteristics = MapToDbCharacteristics(person, databasePerson);

                await _context.People.AddAsync(databasePerson, cancellation);
                await _context.PersonCharacteristics
                    .AddRangeAsync(characteristics, cancellation);
                await _context.SaveChangesAsync(cancellation);

                return await _mapper.DomainPerson(databasePerson, cancellation);
            }
            catch (System.Exception ex)
            {
                throw HandleException(ex, person);
            }
        }

        public async Task<DomainPerson> UpdatePersonAsync
            (DomainPerson person, CancellationToken cancellation)
        {
            await UniqueValuesHandlerAsync(person, false, cancellation);
            try
            {
                var databasePerson = await GetDbPersonAsync(person.Id, cancellation);
                databasePerson.PersonCharacteristics.Clear();

                databasePerson = MapToDbPerson(person, databasePerson);
                var characteristics = MapToDbCharacteristics(person, databasePerson);

                await _context.PersonCharacteristics
                    .AddRangeAsync(characteristics, cancellation);
                await _context.SaveChangesAsync(cancellation);

                return await _mapper.DomainPerson(databasePerson, cancellation);
            }
            catch (System.Exception ex)
            {
                throw HandleException(ex, person);
            }
        }

        //DQL
        public async Task<DomainPerson> GetPersonAsync(UserId id, CancellationToken cancellation)
        {
            var databasePerson = await GetDbPersonAsync(id, cancellation);
            return await _mapper.DomainPerson(databasePerson, cancellation);
        }

        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Pivate Methods
        private async Task<Person> GetDbPersonAsync
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
                    Messages.Person_Cmd_Id_NotFound,
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return databasePerson;
        }

        private Person MapToDbPerson(DomainPerson person, Person? dbPerson = null)
        {
            var database = dbPerson ?? new Person();
            database.UserId = person.Id.Value;
            database.UrlSegment = person.UrlSegment?.Value;
            database.ContactEmail = person.ContactEmail.Value;
            database.Name = person.Name;
            database.Surname = person.Surname;
            database.BirthDate = person.BirthDate == null ? null : person.BirthDate.Value;
            database.ContactPhoneNum = person.ContactPhoneNum?.Value;
            database.Description = person.Description;
            database.IsStudent = person.IsStudent.Code;
            database.IsPublicProfile = person.IsPublicProfile.Code;
            database.AddressId = person.AddressId?.Value;

            return database;
        }

        private IEnumerable<PersonCharacteristic> MapToDbCharacteristics
            (DomainPerson domain, Person database)
        {
            return domain.Characteristics.Select(x => new PersonCharacteristic
            {
                Person = database,
                CharacteristicId = x.Value.Item1.Id.Value,
                QualityId = x.Value.Item2?.Id.Value,
            });
        }

        private async Task UniqueValuesHandlerAsync
            (DomainPerson domain, bool isForCreating, CancellationToken cancellation)
        {
            var query = _context.People.Where(x =>
                (
                    x.ContactEmail != null &&
                    x.ContactEmail == domain.ContactEmail.Value
                ) || (
                    x.ContactPhoneNum != null &&
                    domain.ContactPhoneNum != null &&
                    x.ContactPhoneNum == domain.ContactPhoneNum.Value
                ) || (
                    x.UrlSegment != null &&
                    domain.UrlSegment != null &&
                    x.UrlSegment == domain.UrlSegment.Value
                ))
                .AsQueryable();

            if (!isForCreating)
            {
                query = query.Where(x => x.UserId != domain.Id.Value);
            }

            var duplicates = await query.ToListAsync(cancellation);
            if (duplicates.Any())
            {
                TrowPersonException(duplicates, domain);
            }
        }

        private void TrowPersonException(IEnumerable<Person> duplicates, DomainPerson domain)
        {
            bool isNotUniqueContactEmail = false;
            bool isNotUniqueContactPhoneNum = false;
            bool isNotUniqueUrlSegment = false;

            foreach (var item in duplicates)
            {
                if (item.ContactEmail == domain.ContactEmail.Value)
                {
                    isNotUniqueContactEmail = true;
                }
                if (domain.ContactPhoneNum != null && item.ContactPhoneNum == domain.ContactPhoneNum.Value)
                {
                    isNotUniqueContactPhoneNum = true;
                }
                if (domain.UrlSegment != null && item.UrlSegment == domain.UrlSegment.Value)
                {
                    isNotUniqueUrlSegment = true;
                }
            }

            var builder = new StringBuilder();
            if (isNotUniqueContactEmail)
            {
                builder.AppendLine(Messages.Person_Cmd_NotUniqueContactEmail);
            }
            if (isNotUniqueContactPhoneNum)
            {
                builder.AppendLine(Messages.Person_Cmd_NotUniqueContactPhoneNum);
            }
            if (isNotUniqueUrlSegment)
            {
                builder.AppendLine(Messages.Person_Cmd_NotUniqueUrlSegment);
            }

            throw new PersonException(builder.ToString());
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
