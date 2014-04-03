using System;
using System.Linq;
using System.Linq.Expressions;

namespace Kilo.Data
{
	public interface IRepository<T> : IWriteableRepository<T>
	{
		/// <summary>
		/// Gets a single entity
		/// </summary>
		/// <param name="key">The key.</param>
		T GetSingle(object key);

		/// <summary>
		/// Queries all the entities
		/// </summary>
		IQueryable<T> All();

		/// <summary>
		/// Gets all the data using the specified predicate
		/// </summary>
		IQueryable<T> All(Expression<Func<T, bool>> predicate);

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

		/// <summary>
		/// Gets the object key.
		/// </summary>
		/// <param name="entity">The entity.</param>
		object GetObjectKey(T entity);
	}
}
