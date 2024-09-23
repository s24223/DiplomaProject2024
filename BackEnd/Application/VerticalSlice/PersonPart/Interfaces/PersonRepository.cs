using Application.Database;
using Domain.Entities.PersonPart;
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

        public async Task CreatePersonProfileAsync(DomainPerson person, CancellationToken cancellation)
        {
            var databasePerson = await _context.People
                .Where(x => x.UserId == person.Id.Value)
                .FirstOrDefaultAsync(cancellation);

            if (databasePerson != null)
            {
                //exception
            }
            var databaseUser = await _context.Users
               .Where(x => x.Id == person.Id.Value)
               .FirstOrDefaultAsync(cancellation);

            if (databaseUser == null)
            {
                throw new UnauthorizedAccessException();
                //exception
            }

            //Url Segment, email unique
            await _context.People.AddAsync(new Database.Models.Person
            {
                User = databaseUser,
                UrlSegment = (person.UrlSegment == null) ? null : person.UrlSegment.Value,
                CreateDate = person.CreateDate,
                ContactEmail = person.ContactEmail.Value,
                Name = person.Name,
                Surname = person.Surname,
                BirthDate = (person.BirthDate == null) ? null : person.BirthDate.Value,
                ContactPhoneNum = (person.ContactPhoneNum == null) ? null : person.ContactPhoneNum.Value,
                Description = person.Description,
                IsStudent = person.IsStudent.Code, //person.IsStudent, // tu w Domain person jest bool a w Person string jak to zamienic
                IsPublicProfile = person.IsPublicProfile.Code,// person.IsPublicProfile
            }, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
