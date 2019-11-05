﻿using MvvmCross.Plugin.Messenger;
using Realms;
using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using StreamEncryptor.Predefined;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vault.Core.Model.DbContext;
using Vault.Core.Model.Messages;

namespace Vault.Core.Services
{
    public class ImportService : IImportService
    {
        public const double MAX_THUMB_SIZE = 256d;
        public static readonly string IMAGE_FOLDER_PATH = App.GetAppdataFilePath("0");
        public static readonly string VIDEO_FOLDER_PATH = App.GetAppdataFilePath("1");

        private readonly IMvxMessenger _messenger;
        private readonly Realm _realm;

        public ImportService(IMvxMessenger messenger)
        {
            _messenger = messenger;
            _realm = RealmHelpers.GetRealmInstance();
        }

        public async Task<bool> TryImportImageAsync(string path)
        {
            Media media = null;
            bool success = await Task.Run(() =>
            {
                // Check that the image store folder exists, and try to create one if not
                if (!CheckDirectoryExists(IMAGE_FOLDER_PATH))
                    return false;

                // Check that the file to import exists
                if (!File.Exists(path))
                {
                    Log.Information("Import - Could not find the image to import: {image}", path);
                    ShowErrorDialog("Could not locate the file to import. Please check it is not being used by any other programs");
                    return false;
                }

                string errMessage = string.Empty;
                try
                {
                    // Create the new media store
                    int id = RealmHelpers.GetNextId<Media>();
                    media = new Media(id, MediaType.Image)
                    {
                        FilePath = GetFilePath(IMAGE_FOLDER_PATH, id),
                        Name = Path.GetFileName(path),
                        ThumbPath = GetThumbPath(IMAGE_FOLDER_PATH, id)
                    };

                    // Load, encrypt and save the file and thumbnail
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    using (FileStream output = new FileStream(media.FilePath, FileMode.CreateNew, FileAccess.ReadWrite))
                    using (Image image = Image.Load(fs))
                    using (MemoryStream thumbStore = new MemoryStream())
                    using (FileStream thumbOutput = new FileStream(media.ThumbPath, FileMode.CreateNew, FileAccess.ReadWrite))
                    using (var encryptor = new AesHmacEncryptor("V7GAe5ZRJ4GtxZ3S8jJLCZNQP2SXTyO4"))
                    {
                        // Reset the position after loading the image
                        fs.Position = 0;
                        // Encrypt the original image
                        encryptor.EncryptAsync(fs, output).Wait();

                        // Find the thumbnail scalar
                        double scalar;
                        if (image.Width < image.Height)
                            scalar = MAX_THUMB_SIZE / image.Height;
                        else
                            scalar = MAX_THUMB_SIZE / image.Width;

                        // Mutate and encrypt the image
                        image.Mutate(x => x.Resize((int)(image.Width * scalar), (int)(image.Height * scalar)));
                        image.Save(thumbStore, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
                        thumbStore.Position = 0;
                        encryptor.EncryptAsync(thumbStore, thumbOutput).Wait();
                    }
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
                }
                else
                {
                    return true;
                }
            }).ConfigureAwait(true);

            // Add the media to the realm if required
            if (success)
                await _realm.WriteAsync((r) => r.Add(media)).ConfigureAwait(true);
            return success;
        }

        public Task<bool> TryImportVideoAsync(string path)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> TryRemoveMediaAsync(Media item)
        {
            // Check that the file to import exists
            if (!File.Exists(item.FilePath) || !File.Exists(item.ThumbPath))
            {
                Log.Information("Remove - Could not find the image to remove: {id}", item.Id);
                ShowErrorDialog("An error occured while removing the file");
                return false;
            }

            try
            {
                string filePath = item.FilePath;
                string thumbPath = item.ThumbPath;
                int id = item.Id;
                await Task.Run(() =>
                {
                    File.Delete(filePath);
                    File.Delete(thumbPath);
                }).ConfigureAwait(true);

                await _realm.WriteAsync((r) => r.Remove(r.All<Media>().First(m => m.Id == id))).ConfigureAwait(true);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error removing image");
                ShowErrorDialog("An error occured while removing the file");
                return false;
            }
        }

        /// <summary>
        /// Shows an error dialog to the user
        /// </summary>
        /// <param name="errorMessage">The message to show in the dialog</param>
        private void ShowErrorDialog(string errorMessage)
        {
            _messenger.Publish(new DialogMessage(this, "Import Error", errorMessage, DialogMessage.DialogMessageType.Error));
        }

        #region Filesystem Helpers

        /// <summary>
        /// Checks that a directory exists, and if not creates the directory
        /// </summary>
        /// <param name="directoryPath">The path to the directory</param>
        /// <returns></returns>
        private bool CheckDirectoryExists(string directoryPath)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                return true;
            }
            catch (Exception ex)
            {
                App.LogError("Could not import image", ex);
                return false;
            }
        }

        /// <summary>
        /// Gets the path to a vault file
        /// </summary>
        /// <param name="mediaDirectory">The path to the directory of that media type</param>
        /// <param name="id">The id of the file</param>
        /// <returns></returns>
        private string GetFilePath(string mediaDirectory, int id)
        {
            return Path.Combine(mediaDirectory, id.ToString() + ".vault");
        }

        /// <summary>
        /// Gets the path to a vault thumbnail
        /// </summary>
        /// <param name="mediaDirectory">The path to the directory of that media type</param>
        /// <param name="id">The id of the thumbnail's primary file</param>
        /// <returns></returns>
        private string GetThumbPath(string mediaDirectory, int id)
        {
            return Path.Combine(mediaDirectory, id.ToString() + ".vaultt");
        }

        #endregion
    }
}
