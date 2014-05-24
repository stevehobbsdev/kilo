using System;
using System.Linq;
using System.Linq.Expressions;

namespace Kilo.Data
{
    public interface IQueryableRepository<T, TKey>
    {
        /// <summary>
        /// Gets a single entity
        /// </summary>
        /// <param name="key">The key.</param>
        T Single(TKey key);

        /// <summary>
        /// Gets all the data using the specified predicate
        /// </summary>
        IQueryable<T> Query(params Expression<Func<T, bool>>[] predicates);
    }

    public interface IQueryableRepository<TTable, TDomain, TKey>
	{
		/// <summary>
		/// Gets a single entity
		/// </summary>
		/// <param name="key">The key.</param>
        TDomain Single(TKey key);

		/// <summary>
		/// Gets all the data using the specified predicate
		/// </summary>
        IQueryable<TDomain> Query(params Expression<Func<TTable, bool>>[] predicates);
	}
}
