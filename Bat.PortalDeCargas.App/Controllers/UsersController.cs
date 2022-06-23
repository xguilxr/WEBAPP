using System;
using System.Linq;
using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bat.PortalDeCargas.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService, ILogger<UsersController> logger)
        {
            _usersService = usersService;
            _logger = logger;
        }

        [HttpGet("filtered")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetFiltered([FromQuery] string name, [FromQuery] int page = 1)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values.Select(v => v.Errors.FirstOrDefault()));
                }

                return Ok(await _usersService.GetAll(name, page));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro obtendo usuários");

                throw;
            }
        }

        [HttpPost("save")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Post([FromBody] UserFormDTO user)
        {
            try
            {
                return Ok(await _usersService.Save(user));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro criando usuário");

                throw;
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var result = await _usersService.Delete(id);

                return result.IsValid ? Ok() : (IActionResult)NotFound(result.Errors.Distinct());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro removendo usuário");

                throw;
            }
        }

        [HttpPost("changePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO password)
        {
            try
            {
                return Ok(await _usersService.ChangePassword(password));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro trocando a senha do usuário");

                throw;
            }
        }

        [HttpGet]
        [Route("GetUserById/{userId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                return Ok(await _usersService.GetUserById(userId));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro obtendo usuário");

                throw;
            }
        }
    }
}
