namespace Bat.PortalDeCargas.Domain.Services.UserService
{
    public interface IPasswordService
    {
        string CriptPassword(string password);
        bool ValidatePassword(string password, string hash);
    }
}