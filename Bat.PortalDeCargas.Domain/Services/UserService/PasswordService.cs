namespace Bat.PortalDeCargas.Domain.Services.UserService
{
    public class PasswordService : IPasswordService
    {
        public string CriptPassword(string password)
        {
            
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool ValidatePassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}