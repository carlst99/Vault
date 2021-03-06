﻿using Realms;

namespace Vault.Core.Model.DbContext
{
    public class Preferences : RealmObject
    {
        public bool DarkModeEnabled { get; set; }
        public int ImageThumbnailSize { get; set; }

        public Preferences()
        {
            DarkModeEnabled = false;
            ImageThumbnailSize = 128;
        }
    }
}
