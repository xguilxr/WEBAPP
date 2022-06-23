using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bat.PortalDeCargas.Domain.Repositories
{
    public  interface IUsersRepository:IRepository<AppUser>
    {
        Task<IEnumerable<PaginatedUsersDTO>> GetPaginatedUsers(string name, int page, int pageCount);
        Task<AppUser> AddUser(AppUser User);
        Task<int> Delete(int userId);
        Task<AppUser> GetUserByEmail(string email);
        Task<AppUser> UpdateUser(AppUser User);
        Task<AppUser> GetUserById(int UserId);
        Task<int> ChangePassword(ChangePasswordDTO changePasswordData);

    }
}
