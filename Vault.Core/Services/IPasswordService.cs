using System.Threading.Tasks;

namespace Vault.Core.Services
{
    public interface IPasswordService
    {
        Task<bool> TryVerifyPasswordAsync(string password);
        Task<bool> TryChangePasswordAsync(string oldPassword, string newPassword);
    }
}
