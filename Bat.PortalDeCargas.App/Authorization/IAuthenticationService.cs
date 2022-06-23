using System.IdentityModel.Tokens.Jwt;
using Bat.PortalDeCargas.Domain.Entities;

namespace Bat.PortalDeCargas.Domain.Services.UserService
{
    public interface IAuthenticationService
    {
        AuthenticationResponse Authenticate(AppUser user, string password);
        JwtSecurityToken ValidateJwtToken(string token);
        string GenerateJwtToken(AppUser user);
    }
}
