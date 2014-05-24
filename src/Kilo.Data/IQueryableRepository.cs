using System;
using System.Linq;
using System.Linq.Expressions;

namespace Kilo.Data
{
    public interface IQueryableRepository<T>
    {
        /// <summary>
        /// Gets a single entity
        /// </summary>
        /// <param name="key">The key.</param>
        T Single(dynamic key);

        /// <summary>
        /// Gets all the data using the specified predicate
        /// </summary>
        IQueryable<T> Query(params Expression<Func<T, bool>>[] predicates);
    }

    public interface IQueryableRepository<TTable, TDomain>
	{
		/// <summary>
		/// Gets a single entity
		/// </summary>
		/// <param name="key">The key.</param>
        TDomain Single(dynamic key);

		/// <summary>
		/// Gets all the data using the specified predicate
		/// </summary>
        IQueryable<TDomain> Query(params Expression<Func<TTable, bool>>[] predicates);
	}
}
