using System.Threading.Tasks;

namespace Vault.Core.Services
{
    public interface IPasswordService
    {
        Task<bool> TryVerifyPasswordAsync(string password);
        Task<PasswordChangeResult> TryChangePasswordAsync(string oldPassword, string newPassword);
    }

    public enum PasswordChangeResult
    {
        OldPasswordIncorrect,
        Success,
        Failure
    }
}
