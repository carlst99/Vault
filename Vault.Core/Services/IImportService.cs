using System.Threading.Tasks;
using Vault.Core.Model.DbContext;

namespace Vault.Core.Services
{
    public interface IImportService
    {
        Task<bool> TryImportImageAsync(string path);
        Task<bool> TryImportVideoAsync(string path);
        Task<bool> TryRemoveMediaAsync(Media item);
    }
}
