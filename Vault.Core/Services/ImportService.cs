using IntraMessaging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Vault.Core.Model.Messages;

namespace Vault.Core.Services
{
    public class ImportService : IImportService
    {
        public const string IMAGE_FOLDER_NAME = "0";
        public const string VIDEO_FOLDER_NAME = "0";

        private readonly IIntraMessenger _messenger;

        public ImportService(IIntraMessenger messenger)
        {
            _messenger = messenger;
        }

        public bool TryImportImage(string path)
        {
            if (!CheckImportFolderStructure(App.GetAppdataFilePath(IMAGE_FOLDER_NAME)))
                return false;

            if (!File.Exists(path))
            {
                Log.Information("Import - Could not find the image to import: {image}", path);
                return false;
            }

            string errMessage = string.Empty;
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {

                }
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

        public bool TryImportVideo(string path)
        {
            throw new NotImplementedException();
        }

        private bool CheckImportFolderStructure(string folderPath)
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
            _messenger.Send(new DialogMessage
            {
                Title = "Import Error",
                Content = errorMessage,
                Buttons = DialogMessage.DialogButton.Ok
            });
        }
    }
}
