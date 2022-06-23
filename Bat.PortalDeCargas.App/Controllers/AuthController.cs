using System;
using System.Linq;
using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.DTO;
using Bat.PortalDeCargas.Domain.Entities;
using Bat.PortalDeCargas.Domain.Enums;
using Bat.PortalDeCargas.Domain.Services.UserService;
using Bat.PortalDeCargas.Domain.Services.UserServices;
using Bat.PortalDeCargas.Resource.Translation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Bat.PortalDeCargas.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthController> _logger;
        private readonly IStringLocalizer<AuthTranslation> _stringLocalizer;
        private readonly IUsersService _usersService;

        public AuthController
        (IUsersService usersService, ILogger<AuthController> logger,
            IStringLocalizer<AuthTranslation> stringLocalizer, IAuthenticationService authenticationService)
        {
            _usersService = usersService;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
            _authenticationService = authenticationService;
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromForm] string token)
        {
            try
            {
                var auth = _authenticationService.ValidateJwtToken(token);

                if (auth != null)
                {
                    var email = auth.Claims.SingleOrDefault(c => c.Type == "email")?.Value;
                    var user = await _usersService.GetUserByEmail(email);
                    var newToken = _authenticationService.GenerateJwtToken(user);

                    return Ok(new AuthenticationResponse
                    {
                        Email = user.UserEmail,
                        Token = newToken,
                        UserId = user.UserId,
                        UserName = user.UserName,
                        Role = user.UserType.RoleName()
                    });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro renovando o Token");

                throw;
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var user = await _usersService.GetUserByEmail(loginDto.Email);
                var authResponse = _authenticationService.Authenticate(user, loginDto.Password);

                return authResponse == null ? (IActionResult)Unauthorized() : Ok(authResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro efetuando o login");

                return Unauthorized();
            }
        }
    }
}
