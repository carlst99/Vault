using MvvmCross.Plugin.Messenger;
using System;

namespace Vault.Core.Model.Messages
{
    public class DialogMessage : MvxMessage
    {
        #region Enums

        [Flags]
        public enum DialogButton
        {
            None = 0,
            Ok = 2,
            Yes = 4,
            No = 8,
            Cancel = 16
        }

        public enum DialogMessageType
        {
            Error = 0,
            Info = 1,
            Interaction = 2
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the title to display in the message box
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the content to display in the message box
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the buttons to use in the message box
        /// </summary>
        [Obsolete]
        public DialogButton Buttons { get; set; }

        /// <summary>
        /// Gets or sets a callback, to be used when <see cref="DialogMessageType.Interaction"/> is requested
        /// </summary>
        public Action<DialogButton> Callback { get; set; }

        /// <summary>
        /// Gets or sets the type of message being delivered to the user
        /// </summary>
        public DialogMessageType MessageType { get; set; }

        #endregion

        public DialogMessage(object sender, string title, string content, DialogMessageType messageType)
            : base (sender)
        {
            Title = title;
            Content = content;
            MessageType = messageType;
        }
    }
}
