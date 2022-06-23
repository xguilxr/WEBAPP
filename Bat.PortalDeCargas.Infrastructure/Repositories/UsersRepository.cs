using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Entities;
using Bat.PortalDeCargas.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bat.PortalDeCargas.Infrastructure.Repositories
{
    public class UsersRepository : Repository<AppUser>, IUsersRepository
    {
        internal UsersRepository(PortalDeCargasContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<PaginatedUsersDTO>> GetPaginatedUsers(string name, int page, int pageCount)
        {

            object parameters = new { UserName = name, CurrentPage = page, ItemsPerPage = pageCount };

            return await Query<PaginatedUsersDTO>("up_GetPaginatedUsers", parameters);
        }

        public async Task<int> Delete(int userId)
        {
            return await Remove(new { userId });
        }

        public async Task<AppUser> GetUserByEmail(string email)
        {            
            object parameters = new { UserEmail = email };

            return await SingleOrDefault<AppUser>("up_GetFilteredUsers", parameters);
        }

        public async Task<AppUser> GetUserById(int userId)
        {
            object parameters = new {
                UserId = userId };

            return await FirstOrDefault<AppUser>("up_GetFilteredUsers", parameters);
        }

        public async Task<AppUser> AddUser(AppUser user)
        {
            return await Add(new
            {
                user.UserName,
                user.UserEmail,
                user.UserType,
                user.Password
            });
        }

        public async Task<AppUser> UpdateUser(AppUser user)
        {
            return await Update<AppUser>(new
                {
                    user.UserId,
                    user.UserName,
                    user.UserEmail,
                    user.UserType
                });
        }

        public async Task<int> ChangePassword(ChangePasswordDTO changePasswordData)
        {
            object parameters = new
            {
                changePasswordData.UserId,
                changePasswordData.Password
            };

            return await Execute("up_ChangePassword", parameters);
        }
    }
}
