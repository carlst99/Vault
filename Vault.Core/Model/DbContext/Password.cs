using Realms;

namespace Vault.Core.Model.DbContext
{
    public class Password : RealmObject
    {
        public string Hash { get; set; }
        //private byte[] Hash { get; set; }
        //private byte[] Salt { get; set; }

        //public Span<byte> HashWrapper
        //{
        //    get => Hash;
        //    set => Hash = value.ToArray();
        //}

        //public Span<byte> SaltWrapper
        //{
        //    get => Salt;
        //    set => Salt = value.ToArray();
        //}
    }
}
