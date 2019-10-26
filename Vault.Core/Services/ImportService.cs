using MvvmCross.Plugin.Messenger;
using Realms;
using Serilog;
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

        private readonly IMvxMessenger _messenger;
        private readonly Realm _realm;

        public ImportService(IMvxMessenger messenger)
        {
            _messenger = messenger;
            _realm = RealmHelpers.GetRealmInstance();
        }

        public async Task<bool> TryImportImageAsync(string path)
        {
            string outputFolder = App.GetAppdataFilePath(IMAGE_FOLDER_NAME);
            if (!PreImportChecks(outputFolder))
                return false;

            if (!File.Exists(path))
            {
                Log.Information("Import - Could not find the image to import: {image}", path);
                return false;
            }

            string errMessage = string.Empty;
            try
            {
                int id = RealmHelpers.GetNextId<Media>();
                Media media = new Media(id, MediaType.Image)
                {
                    FilePath = GetFilePath(outputFolder, id),
                    Name = Path.GetFileName(path),
                    ThumbPath = GetThumbPath(outputFolder, id)
                };

                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (FileStream output = new FileStream(media.FilePath, FileMode.CreateNew, FileAccess.Write))
                using (var encryptor = new AesHmacEncryptor("V7GAe5ZRJ4GtxZ3S8jJLCZNQP2SXTyO4"))
                {
                    await encryptor.EncryptAsync(fs, output).ConfigureAwait(false);
                }

                await _realm.WriteAsync((r) => r.Add(media));
            }
            catch (FileNotFoundException fnfex)
            {
                errMessage = "Could not locate the file to import. Please try again!";
                App.LogError("Error importing image", fnfex);
            }
            catch (IOException ioex)
            {
                errMessage = "An error occured while importing. Please try again!";
                App.LogError("Error importing image", ioex);
            }
            catch (UnauthorizedAccessException uaex)
            {
                errMessage = "You don't have access to that file, sorry!";
                App.LogError("Error importing image", uaex);
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
            return Path.Combine(folder, id.ToString(), ".vault");
        }

        private string GetThumbPath(string folder, int id)
        {
            return Path.Combine(folder, id.ToString(), ".vaultt");
        }
    }
}
