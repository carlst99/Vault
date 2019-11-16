using MvvmCross.Plugin.Messenger;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vault.Core.Model.Messages
{
    public class OpenFileDialogMessage : MvxMessage
    {
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

        public string Title { get; set; }
        public List<Tuple<string, string>> Filters { get; set; }
        public Action<bool, IEnumerable<string>> Callback { get; set; }
        public bool MultiSelect { get; set; }

        public OpenFileDialogMessage(object sender, string title, List<Tuple<string, string>> filters, bool multiSelect = true)
            : base(sender)
        {
            Title = title;
            Filters = filters;
            MultiSelect = multiSelect;
        }
    }
}
