using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Persons.Mappers;
using Domain.Features.Person.Entities;
using Domain.Features.Person.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Microsoft.Data.SqlClient;
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
                Console.WriteLine(ex);
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
                databasePerson = MapToDbPerson(person, databasePerson);

                var characteristics = MapToDbCharacteristics(person, databasePerson);
                _context.PersonCharacteristics
                    .RemoveRange(databasePerson.PersonCharacteristics);
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
            if (dbPerson == null)
            {
                database.UserId = person.Id.Value;
            }
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
                )).Where(x => x.UserId != domain.Id.Value)
                .AsQueryable();


            var duplicates = await query.ToListAsync(cancellation);
            if (duplicates.Any())
            {
                ThrowPersonException(duplicates, domain);
            }
        }

        private void ThrowPersonException(IEnumerable<Person> duplicates, DomainPerson domain)
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
                if (isNotUniqueContactEmail && isNotUniqueContactPhoneNum && isNotUniqueUrlSegment)
                {
                    break;
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
             */
            //2627  Unique, Pk 
            //547   Check, FK
            var dictionary = new Dictionary<string, string>()
            {
                { "Person_CHECK_IsStudent",$"{Messages.Person_Cmd_IncorrectIsStudent}: {domain.IsStudent.Code}"},
                { "Person_CHECK_IsPublicProfile",$"{Messages.Person_Cmd_IncorrectIsPublicProfile}: {domain.IsPublicProfile.Code}"},
                { "Person_CHECK_BirthDate",$"{Messages.Person_Cmd_IncorrectBirthDate}: {domain.BirthDate?.ToString()}"},
            };

            if (ex is DbUpdateException && ex.InnerException is SqlException sqlEx)
            {
                var number = sqlEx.Number;
                var message = sqlEx.Message;
                if (number == 2627)
                {
                    if (message.Contains("Person_pk"))
                    {
                        return new PersonException(Messages.Person_Cmd_ExistProfile);
                    }
                }

                if (number == 547)
                {
                    if (message.Contains("Person_Address"))
                    {
                        return new PersonException
                                (
                                $"{Messages.Person_Cmd_Address_NotFound}: {domain.AddressId?.ToString()}",
                                DomainExceptionTypeEnum.NotFound
                                );
                    }

                    foreach (var item in dictionary)
                    {
                        if (message.Contains(item.Key))
                        {
                            return new PersonException
                                (
                                item.Value,
                                DomainExceptionTypeEnum.AppProblem
                                );
                        }
                    }
                }
            }
            return ex;
        }
    }
}
