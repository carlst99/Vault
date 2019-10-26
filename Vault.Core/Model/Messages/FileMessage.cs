using MvvmCross.Plugin.Messenger;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vault.Core.Model.Messages
{
    public class FileMessage : MvxMessage
    {
        #region Enums

        public enum DialogType
        {
            OpenFile, OpenFolder, SaveFile
        }

        public enum DialogResult
        {
            Failed, Succeeded
        }

        #endregion

        #region Default filters

        public static readonly List<Tuple<string, string>> DefaultImageFilters = new List<Tuple<string, string>>
        {
            new Tuple<string, string>("All Files", "*.*"),
            new Tuple<string, string>("PNG Files", "*.png"),
            new Tuple<string, string>("JPG Files", "*.jpg"),
            new Tuple<string, string>("GIF Files", "*.gif")
        };

        public static readonly List<Tuple<string, string>> DefaultVideoFilters = new List<Tuple<string, string>>
        {
            new Tuple<string, string>("All Files", "*.*"),
            new Tuple<string, string>("MP4 Files", "*.mp4"),
            new Tuple<string, string>("WEBM Files", "*.webm"),
            new Tuple<string, string>("AVI Files", "*.avi"),
        };

        #endregion

        public DialogType Type { get; set; }
        public string Title { get; set; }
        public List<Tuple<string, string>> Filters { get; set; }
        public Action<DialogResult, IEnumerable<string>> Callback { get; set; }
        public bool MultiSelect { get; set; }

        public FileMessage(object sender, string title, DialogType type, List<Tuple<string, string>> filters)
            : base (sender)
        {
            Title = title;
            Type = type;
            Filters = filters;
            MultiSelect = true;
        }
    }
}
