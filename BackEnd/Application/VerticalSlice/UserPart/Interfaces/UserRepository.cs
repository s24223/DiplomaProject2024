using Application.Database;
using Domain.Entities.UserPart;
using Domain.ValueObjects.ValueEmail;
using Microsoft.EntityFrameworkCore;

namespace Application.VerticalSlice.UserPart.Interfaces
{
    public class UserRepository : IUserRepository
    {
        private readonly DiplomaProjectContext _context;


        public UserRepository
            (
            DiplomaProjectContext context
            )
        {
            _context = context;
        }

        public async Task<bool> IsExistLoginEmailAsync
            (
            Email loginEmail,
            CancellationToken cancellation
            )
        {
            var result = await _context.Users.Where(x => x.LoginEmail == loginEmail.Value)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellation);
            return result != null;
        }

        public async Task SetUserAsync
            (
            User domainUser,
            string password,
            string salt,
            DateTime now,
            CancellationToken cancellation
            )
        {
            var id = Guid.NewGuid();
            var userWithSameId = await _context.Users.FindAsync(id);

            while (userWithSameId != null)
            {
                id = Guid.NewGuid();
                userWithSameId = await _context.Users.FindAsync(id);
            }

            await _context.Users.AddAsync(new Database.Models.User
            {
                Id = id,
                LoginEmail = domainUser.LoginEmail.Value,
                Password = password,
                Salt = salt,
                LastUpdatePassword = now,
            });
            await _context.SaveChangesAsync();
        }

        public async Task<Application.Database.Models.User?> GetUserByLoginEmailAsync
            (
            string loginEmail,
            CancellationToken cancellation
            )
        {
            return await _context.Users.Where(x => x.LoginEmail == loginEmail)
                .Include(x => x.Person)
                .Include(x => x.Company)
                .FirstOrDefaultAsync(cancellation);
        }

        public async Task<Application.Database.Models.User?> GetUserByIdAsync
            (
            Guid id,
            CancellationToken cancellation
            )
        {
            return await _context.Users.Where(x => x.Id == id)
                .Include(x => x.Person)
                .Include(x => x.Company)
                .FirstOrDefaultAsync(cancellation);
        }

        public async Task SetRefreshTokenDataLoginInAsync
            (
            Application.Database.Models.User user,
            string refresh,
            DateTime validTo,
            DateTime now
            )
        {
            user.RefreshToken = refresh;
            user.ExpiredToken = validTo;
            user.LastLoginIn = now;

            await _context.SaveChangesAsync();
        }

        public async Task SetRefreshTokenDataLogOutAsync
            (
            Application.Database.Models.User user,
            CancellationToken cancelToken
            )
        {
            user.RefreshToken = null;
            user.ExpiredToken = null;

            await _context.SaveChangesAsync();
        }
    }
}
