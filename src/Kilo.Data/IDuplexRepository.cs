
namespace Kilo.Data
{
    /// <summary>
    /// A repository type that can read and write to a data source
    /// </summary>
    /// <typeparam name="TDomain">The domain entity type</typeparam>
    /// <typeparam name="TKey">The key type</typeparam>
    public interface IDuplexRepository<TDomain, TKey> : IQueryableRepository<TDomain, TKey>, IWriteableRepository<TDomain>
    {
    }

    /// <summary>
    /// A repository type that can read and write to a domain-mapped data source
    /// </summary>
    /// <typeparam name="TTable">The type of the table.</typeparam>
    /// <typeparam name="TDomain">The type of the domain.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IDuplexRepository<TTable, TDomain, TKey> : IQueryableRepository<TTable, TDomain, TKey>, IWriteableRepository<TDomain>
    {
    }
}
