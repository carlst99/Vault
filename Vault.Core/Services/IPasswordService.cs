namespace Vault.Core.Services
{
    public interface IPasswordService
    {
        bool VerifyPassword(string password);
        bool TryChangePassword(string oldPassword, string newPassword);
    }
}
