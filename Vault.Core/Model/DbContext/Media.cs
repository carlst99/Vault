using Realms;

namespace Vault.Core.Model.DbContext
{
    public class Media : RealmObject, IContextItem
    {
        public int TypeRaw { get; set; }

        /// <summary>
        /// Gets or sets the database ID
        /// </summary>
        [PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the full name of the trialist
        /// </summary>
        [Required]
        [Indexed]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of media in the store
        /// </summary>
        public MediaType Type
        {
            get => (MediaType)TypeRaw;
            set => TypeRaw = (int)value;
        }

        /// <summary>
        /// Gets or sets the path to the file
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the path to the thumbnail
        /// </summary>
        public string ThumbPath { get; set; }

        #region Ctors

        public Media() { }

        public Media(int id, MediaType type)
        {
            Id = id;
            Type = type;
        }

        #endregion

        public Media(MediaType type, string name, string filePath, string thumbPath = null)
        {
            Id = RealmHelpers.GetNextId<Media>();
            Type = type;
            Name = name;
            FilePath = filePath;
            ThumbPath = thumbPath;
        }

        #region Object Overrides

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return obj is Media media
                && media.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            const int hash = 13;
            return (hash * 7) + Id;
        }

        #endregion
    }
}
