using System;
using System.IO;
using System.Threading.Tasks;

namespace Vault.Core.Services
{
    public class MediaLoaderService : IMediaLoaderService
    {
        public async Task<Stream> LoadImageAsync(string path)
        {
            async Task<Stream> GetImageNotFound()
            {
                Stream notFoundStream = null;
                await Task.Run(() => notFoundStream = typeof(EncryptorAssistant).Assembly.GetManifestResourceStream("Vault.Core.Resources.ImageNotFound.png")).ConfigureAwait(true);
                return notFoundStream;
            }

            Stream imageStream = new MemoryStream();
            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        await EncryptorAssistant.GetEncryptor().DecryptAsync(fs, imageStream).ConfigureAwait(true);
                        imageStream.Position = 0;
                    }
                    catch (Exception ex)
                    {
                        App.LogError("Could not decrypt image", ex);
                        imageStream = await GetImageNotFound().ConfigureAwait(true);
                    }
                }
            }
            else
            {
                imageStream = await GetImageNotFound().ConfigureAwait(true);
            }
            return imageStream;
        }

        public async Task<Stream> LoadVideoAsync(string path)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> TryUpdateMediaAsync(string path, Stream stream)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                try
                {
                    stream.Position = 0;
                    await EncryptorAssistant.GetEncryptor().EncryptAsync(stream, fs).ConfigureAwait(false);
                    return true;
                }
                catch (Exception ex)
                {
                    App.LogError("Could not update media", ex);
                    return false;
                }
            }
        }
    }
}
