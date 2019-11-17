using MvvmCross.Plugin.Messenger;
using Vault.Core.Model.DbContext;

namespace Vault.Core.Model.Messages
{
    public class MediaUpdatedMessage : MvxMessage
    {
        public enum UpdateType
        {
            Added,
            Updated,
            Removed
        }

        public Media UpdatedMedia { get; }
        public UpdateType Type { get; }

        public MediaUpdatedMessage(object sender, Media updatedMedia, UpdateType type)
            : base(sender)
        {
            UpdatedMedia = updatedMedia;
            Type = type;
        }
    }
}
