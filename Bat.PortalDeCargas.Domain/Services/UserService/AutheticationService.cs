using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.Configuration;
using Bat.PortalDeCargas.Domain.Entities;
using Bat.PortalDeCargas.Domain.Enums;
using Bat.PortalDeCargas.Domain.Services.UserServices;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Bat.PortalDeCargas.Domain.Services.UserService
{
    public class AutheticationService : IAutheticationService
    {
        private readonly IAppConfiguration _appConfig;
        private readonly ILogger<AutheticationService> _logger;
        private readonly IPasswordService _passwordService;
        private readonly IUsersService _usersService;

        public AutheticationService
        (IUsersService usersService, IPasswordService passwordService, IAppConfiguration appConfig,
            ILogger<AutheticationService> logger)
        {
            _usersService = usersService;
            _passwordService = passwordService;
            _appConfig = appConfig;
            _logger = logger;
        }

        public async Task<AuthenticationResponse> Authenticate(string userEmail, string password)
        {
            var user = await _usersService.GetUserByEmail(userEmail);
            var hash = _passwordService.CriptPassword(password);

            if (user.Password.Equals(hash))
            {
                return new AuthenticationResponse
                {
                    Email = user.UserEmail,
                    Token = GenerateJwtToken(user),
                    UserId = user.UserId,
                    UserName = user.UserName
                };
            }

            return null;
        }

        private string GenerateJwtToken(AppUser user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, user.UserEmail), new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.UserType.RoleName()),
                    new Claim(ClaimTypes.Sid, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
