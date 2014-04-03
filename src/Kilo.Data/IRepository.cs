using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects.DataClasses;
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
		/// Attaches the specified entity, optionally dictating the state to attach as.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="state">The state.</param>
		void Attach(T entity, State state = State.Unchanged);

		/// <summary>
		/// Detaches the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Detach(T entity);

		/// <summary>
		/// Saves the changes.
		/// </summary>
		void SaveChanges();

		/// <summary>
		/// Gets the object key.
		/// </summary>
		/// <param name="entity">The entity.</param>
		object GetObjectKey(T entity);
	}
}
