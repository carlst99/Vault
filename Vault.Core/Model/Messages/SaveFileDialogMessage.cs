using MvvmCross.Plugin.Messenger;
using System;

namespace Vault.Core.Model.Messages
{
    public class SaveFileDialogMessage : MvxMessage
    {
        public string Title { get; set; }
        public Action<bool, string> Callback { get; set; }
        public string DefaultFileName { get; set; }
        public string DefaultFileExtension { get; set; }

        public SaveFileDialogMessage(object sender, string title, string fileName, string extension)
            : base(sender)
        {
            Title = title;
            DefaultFileExtension = extension;
            DefaultFileName = fileName;
        }
    }
}
