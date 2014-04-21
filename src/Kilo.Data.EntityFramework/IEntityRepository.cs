using System;
using System.Linq;
using System.Linq.Expressions;

namespace Kilo.Data.EntityFramework
{
    public interface IEntityRepository<T, TKey> : IRepository<T, TKey>
    {
        /// <summary>
        /// Gets the object key.
        /// </summary>
        /// <param name="entity">The entity.</param>
        object GetObjectKey(T entity);

        /// <summary>
        /// Gets all the data, performing preload operations using the specified expression list.
        /// </summary>
        /// <param name="includes">The include expressions</param>
        IQueryable<T> AllWithIncludes(params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Gets all the data, performing preload operations using the specified expression list. The data is filtered using the supplied predicate.
        /// </summary>
        /// <param name="predicate">A filtering predicate to perform on the resulting data set.</param>
        /// <param name="includes">The include expressions</param>
        IQueryable<T> AllWithIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    }
}
