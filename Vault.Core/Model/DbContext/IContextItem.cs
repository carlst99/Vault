using Realms;

namespace Vault.Core.Model.DbContext
{
    public interface IContextItem
    {
        [PrimaryKey]
        int Id { get; set; }
    }
}
