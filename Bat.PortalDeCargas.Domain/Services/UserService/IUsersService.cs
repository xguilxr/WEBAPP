using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Entities;
using Impeto.Framework.Domain.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bat.PortalDeCargas.Domain.Services.UserServices
{
    public interface IUsersService
    {
        Task<PaginationDTO<PaginatedUsersDTO>> GetAll(string name, int page);
        Task<ResultDTO<AppUser>> Save(UserFormDTO User);
        Task<ValidationResult> Delete(int userId);
        Task<AppUser> GetUserById(int userId);
        Task<ResultDTO<ChangePasswordDTO>> ChangePassword(ChangePasswordDTO Password);
        Task<AppUser> GetUserByEmail(string email);
    }
}
