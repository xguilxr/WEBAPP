using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bat.PortalDeCargas.Domain.Configuration;
using Bat.PortalDeCargas.Domain.Entities;
using Bat.PortalDeCargas.Domain.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Bat.PortalDeCargas.Domain.Services.UserService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAppConfiguration _appConfig;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IPasswordService _passwordService;

        public AuthenticationService
            (IPasswordService passwordService, IAppConfiguration appConfig, ILogger<AuthenticationService> logger)
        {
            _passwordService = passwordService;
            _appConfig = appConfig;
            _logger = logger;
        }

        public AuthenticationResponse Authenticate(AppUser user, string password)
        {
            try
            {
                if (_passwordService.ValidatePassword(password, user.Password))
                {
                    return new AuthenticationResponse
                    {
                        Email = user.UserEmail,
                        Token = GenerateJwtToken(user),
                        UserId = user.UserId,
                        UserName = user.UserName,
                        Role = user.UserType.RoleName()
                    };
                }

                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro autenticando o usuário");

                return null;
            }
        }

        public JwtSecurityToken ValidateJwtToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_appConfig.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                return (JwtSecurityToken)validatedToken;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro validando o token");

                return null;
            }
        }

        public string GenerateJwtToken(AppUser user)
        {
            try
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

                var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro gerando o token");

                throw;
            }
        }
    }
}
