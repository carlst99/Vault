using System.Threading.Tasks;
using Vault.Core.Model.DbContext;

namespace Vault.Core.Services
{
    public interface IImportService
    {
        Task<Media> TryImportImageAsync(string path);
        Task<Media> TryImportVideoAsync(string path);
        Task<bool> TryRemoveMediaAsync(Media item);
        Task<bool> TryExportMediaAsync(Media item, string outputPath);
        bool CheckDirectoryExists(string directoryPath);
    }
}
