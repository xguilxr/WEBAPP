using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.Configuration;
using Bat.PortalDeCargas.Domain.Data;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Entities;
using Bat.PortalDeCargas.Domain.Services.UserService;
using Bat.PortalDeCargas.Resource.Translation;
using Impeto.Framework.Domain.Service;
using Microsoft.Extensions.Localization;

namespace Bat.PortalDeCargas.Domain.Services.UserServices
{
    public class UsersService:IUsersService  
    {
        private readonly IUnitOfWork _unityOfWork;
        private readonly IAppConfiguration _appConfig;
        private readonly IStringLocalizer<UsersTranslation> _stringLocalizer;
        private readonly IPasswordService _passwordService;

        public UsersService
        (   IUnitOfWork unityOfWork, 
            IAppConfiguration appConfig,
            IStringLocalizer<UsersTranslation> stringLocalizer,
            IPasswordService passwordService
        )
        {
            _unityOfWork = unityOfWork;
            _appConfig = appConfig;
            _stringLocalizer = stringLocalizer;
            _passwordService = passwordService;
        }
        
        public async Task<PaginationDTO<PaginatedUsersDTO>> GetAll(string name, int page)
        {
            var users = await _unityOfWork.UsersRepository.GetPaginatedUsers(name, page, _appConfig.ClientsPerPageFilter);
           
            var amount = 0;

            if (users.Any())
                amount = users.FirstOrDefault().TotalOfItems;

            return new PaginationDTO<PaginatedUsersDTO>
            {
                Items = users,
                CurrentPage = page,
                ItemsPerPage = _appConfig.ClientsPerPageFilter,
                TotalOfItems = amount
            };

        }

        public async Task<ResultDTO<AppUser>> Save(UserFormDTO user)
        {
            var result = await new UserValidationService(_unityOfWork, _stringLocalizer).Validate(user);
            var userReturn = new AppUser();
            var appUser = GetAppUserFromUserDto(user);

            if (!result.IsValid)
            {
                return new ResultDTO<AppUser>
                {
                    Erros = result.IsValid ? null : result.Errors.Distinct(),
                    IndSucesso = result.IsValid,
                    Model = userReturn
                };
            }

            if (appUser.UserId == 0)
            {
                userReturn = await CreateNewUser(user, appUser, result);
            }
            else
            {
                appUser.Password = null;
                userReturn = await _unityOfWork.UsersRepository.UpdateUser(appUser);
            }

            return new ResultDTO<AppUser>
            {
                Erros = result.IsValid ? null : result.Errors.Distinct(),
                IndSucesso = result.IsValid,
                Model = userReturn
            };
        }

        private async Task<AppUser> CreateNewUser(UserFormDTO user, AppUser appUser, ValidationResult result)
        {
            AppUser userReturn = null;
            var passwordValidate = new ChangePasswordDTO
            {
                ConfirmPassword = user.ConfirmPassword,
                Password = user.Password,
                UserId = user.UserId
            };

            var passwordValidateResult =
                await new PasswordValidationService(_unityOfWork, _stringLocalizer).Validate(passwordValidate);

            if (passwordValidateResult.IsValid)
            {
                appUser.Password = _passwordService.CriptPassword(appUser.Password);
                userReturn = await _unityOfWork.UsersRepository.AddUser(appUser);
            }
            else
            {
                foreach (var erro in passwordValidateResult.Errors)
                    result.Errors.Add(erro);
            }

            return userReturn;
        }

        public async Task<ResultDTO<ChangePasswordDTO>> ChangePassword(ChangePasswordDTO password)
        {
            var result = await new PasswordValidationService(_unityOfWork, _stringLocalizer).Validate(password);

            if (result.IsValid)
            {
                password.Password = _passwordService.CriptPassword(password.Password);
                await _unityOfWork.UsersRepository.ChangePassword(password);
            }

            return new ResultDTO<ChangePasswordDTO>
            {
                Erros = result.IsValid ? null : result.Errors.Distinct(),
                IndSucesso = result.IsValid,
                Model = password
            };
        }

        public async Task<AppUser> GetUserByEmail(string email)
        {
            return await _unityOfWork.UsersRepository.GetUserByEmail(email);
        }

        public async Task<ValidationResult> Delete(int userId)
        {
            var result = (await _unityOfWork.UsersRepository.Delete(userId)) > 0;

            return result ? new ValidationResult() : new ValidationResult(_stringLocalizer["DimensionNotFound"].Value);
        }

        public  Task<AppUser> GetUserById(int userId)
        {
            return _unityOfWork.UsersRepository.GetUserById(userId);
        }

        private AppUser GetAppUserFromUserDto(UserFormDTO user)
        {

            return new AppUser
            {
                UserId = user.UserId,
                Password = user.Password,
                UserEmail = user.UserEmail,
                UserName = user.UserName,
                UserType = user.UserType
            };
        }

    }
}
