namespace Vault.Core.Services
{
    public interface IImportService
    {
        bool TryImportImage(string path);
        bool TryImportVideo(string path);
    }
}
