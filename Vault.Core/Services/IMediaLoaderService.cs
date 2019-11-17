using System.IO;
using System.Threading.Tasks;

namespace Vault.Core.Services
{
    public interface IMediaLoaderService
    {
        Task<Stream> LoadImageAsync(string path);
        Task<Stream> LoadVideoAsync(string path);
        Task<bool> TryUpdateMediaAsync(string path, Stream stream);
    }
}
