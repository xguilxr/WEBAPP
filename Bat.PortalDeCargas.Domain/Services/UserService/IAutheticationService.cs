using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.Entities;

namespace Bat.PortalDeCargas.Domain.Services.UserService
{
    public interface IAutheticationService
    {
        Task<AuthenticationResponse> Authenticate(string userEmail, string password);

    }
}
