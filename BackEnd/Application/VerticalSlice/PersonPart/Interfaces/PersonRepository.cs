using Application.Database;
using Domain.Entities.PersonPart;
using Domain.Exceptions.AppExceptions.EntitiesExceptions;
using Domain.Exceptions.AppExceptions.ValueObjectsExceptions.ValueObjectsExceptions;
using Domain.Exceptions.UserExceptions.ValueObjectsExceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.VerticalSlice.PersonPart.Interfaces
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DiplomaProjectContext _context;
        public PersonRepository(DiplomaProjectContext context)
        {
            _context = context;
        }

        public async Task CreatePersonProfileAsync
            (
            DomainPerson person,
            CancellationToken cancellation
            )
        {
            var databasePerson = await _context.People
                .Where(x => x.UserId == person.Id.Value)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellation);

            if (databasePerson != null)
            {
                throw new PersonException("Messages.IsExistPerson");
            }

            if (person.UrlSegment != null)
            {
                var personWithSameUrlSegment = await _context.People
               .Where(x => x.UrlSegment == person.UrlSegment.Value)
               .AsNoTracking()
               .FirstOrDefaultAsync(cancellation);

                if (personWithSameUrlSegment != null)
                {
                    throw new UrlSegmentException("Messages.NotUniqueUrlSegment");
                }
            }

            var personWithSameContactEmail = await _context.People
           .Where(x => x.UrlSegment == person.ContactEmail.Value)
           .AsNoTracking()
           .FirstOrDefaultAsync(cancellation);

            if (personWithSameContactEmail != null)
            {
                throw new EmailException("Messages.NotUniqueContactEmail");
            }

            //Url1 Segment, email unique
            var inputDatabasePerson = new Database.Models.Person
            {
                UserId = person.Id.Value,
                UrlSegment = (person.UrlSegment == null) ? null : person.UrlSegment.Value,
                Created = person.CreateDate,
                ContactEmail = person.ContactEmail.Value,
                Name = person.Name,
                Surname = person.Surname,
                BirthDate = (person.BirthDate == null) ? null : person.BirthDate.Value,
                ContactPhoneNum = (person.ContactPhoneNum == null) ? null : person.ContactPhoneNum.Value,
                Description = person.Description,
                IsStudent = person.IsStudent.Code, //person.IsStudent, // tu w Domain person jest bool a w Person string jak to zamienic
                IsPublicProfile = person.IsPublicProfile.Code,// person.IsPublicProfile
            };
            await _context.People.AddAsync(inputDatabasePerson, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
