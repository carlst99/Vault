using System.Threading.Tasks;

namespace Vault.Core.Services
{
    public interface IImportService
    {
        Task<bool> TryImportImageAsync(string path);
        Task<bool> TryImportVideoAsync(string path);
    }
}
