using System.Collections.Generic;

namespace Bat.PortalDeCargas.Domain.Entities
{
    public class AuthenticationResponse
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
