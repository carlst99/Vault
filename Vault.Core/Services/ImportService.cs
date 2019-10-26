using MvvmCross.Plugin.Messenger;
using Realms;
using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using StreamEncryptor.Predefined;
using System;
using System.IO;
using System.Threading.Tasks;
using Vault.Core.Model.DbContext;
using Vault.Core.Model.Messages;

namespace Vault.Core.Services
{
    public class ImportService : IImportService
    {
        public const string IMAGE_FOLDER_NAME = "0";
        public const string VIDEO_FOLDER_NAME = "0";
        public const int MAX_THUMB_SIZE = 256;

        private readonly IMvxMessenger _messenger;
        private readonly Realm _realm;

        public ImportService(IMvxMessenger messenger)
        {
            _messenger = messenger;
            _realm = RealmHelpers.GetRealmInstance();
        }

        public async Task<bool> TryImportImageAsync(string path)
        {
            // Check that the image store folder exists
            string outputFolder = App.GetAppdataFilePath(IMAGE_FOLDER_NAME);
            if (!PreImportChecks(outputFolder))
                return false;

            // Check that the file to import exists
            if (!File.Exists(path))
            {
                Log.Information("Import - Could not find the image to import: {image}", path);
                return false;
            }

            string errMessage = string.Empty;
            try
            {
                // Create the new media store
                int id = RealmHelpers.GetNextId<Media>();
                Media media = new Media(id, MediaType.Image)
                {
                    FilePath = GetFilePath(outputFolder, id),
                    Name = Path.GetFileName(path),
                    ThumbPath = GetThumbPath(outputFolder, id)
                };

                // Load, encrypt and save the file and thumbnail
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (FileStream output = new FileStream(media.FilePath, FileMode.CreateNew, FileAccess.Write))
                using (MemoryStream thumbStore = new MemoryStream())
                using (FileStream thumbOutput = new FileStream(media.ThumbPath, FileMode.CreateNew, FileAccess.Write))
                using (Image image = Image.Load(fs))
                using (var encryptor = new AesHmacEncryptor("V7GAe5ZRJ4GtxZ3S8jJLCZNQP2SXTyO4"))
                {
                    // Encrypt the original image
                    await encryptor.EncryptAsync(fs, output).ConfigureAwait(true);

                    // Find the thumbnail scalar
                    int scalar;
                    if (image.Width < image.Height)
                        scalar = MAX_THUMB_SIZE / image.Height;
                    else
                        scalar = MAX_THUMB_SIZE / image.Width;

                    // Mutate and encrypt the image
                    image.Mutate(x => x.Resize(image.Width * scalar, image.Width * scalar));
                    image.Save(thumbStore, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
                    await encryptor.EncryptAsync(thumbStore, thumbOutput).ConfigureAwait(true);
                }

                // Add the media to the realm
                await _realm.WriteAsync((r) => r.Add(media));
            }
            catch (FileNotFoundException fnfex)
            {
                errMessage = "Could not locate the file to import. Please try again!";
                App.LogError("Error importing image", fnfex);
            }
            catch (UnauthorizedAccessException uaex)
            {
                errMessage = "You don't have access to that file, sorry!";
                App.LogError("Error importing image", uaex);
            }
            catch (Exception ex)
            {
                errMessage = "An error occured while importing. Please try again!";
                App.LogError("Error importing image", ex);
            }

            if (!string.IsNullOrEmpty(errMessage))
            {
                ShowErrorDialog(errMessage);
                return false;
            } else
            {
                return true;
            }
        }

        public Task<bool> TryImportVideoAsync(string path)
        {
            throw new NotImplementedException();
        }

        private bool PreImportChecks(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                return true;
            }
            catch (Exception ex)
            {
                App.LogError("Could not import image", ex);
                return false;
            }
        }

        private void ShowErrorDialog(string errorMessage)
        {
            _messenger.Publish(new DialogMessage(this, "Import Error", errorMessage, DialogMessage.DialogMessageType.Error));
        }

        private string GetFilePath(string folder, int id)
        {
            return Path.Combine(folder, id.ToString() + ".vault");
        }

        private string GetThumbPath(string folder, int id)
        {
            return Path.Combine(folder, id.ToString() + ".vaultt");
        }
    }
}
