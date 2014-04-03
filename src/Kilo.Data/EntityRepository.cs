using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Reflection;
using Kilo.Expressions;

namespace Kilo.Data
{
	public class EntityRepository<T> : IRepository<T>
		where T : class, IEntityWithKey
	{
		public ObjectContext _context = null;
		ObjectSet<T> _objectSet;

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityRepository&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="tableName">Name of the entity set.</param>
		/// <param name="entityKeyName">Name of the entity key.</param>
		public EntityRepository(string connectionString)
		{
			_context = new ObjectContext(connectionString);
			_objectSet = _context.CreateObjectSet<T>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityRepository&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="entityKeyName">Name of the entity key.</param>
		public EntityRepository(ObjectContext context)
		{
			_context = context;
			_objectSet = _context.CreateObjectSet<T>();
		}

		/// <summary>
		/// Gets a single entity
		/// </summary>
		/// <param name="key">The key.</param>
		public T GetSingle(object key)
		{
            EntityKey ekey = new EntityKey(_objectSet.EntitySet.Name, _objectSet.EntitySet.ElementType.KeyMembers.First().Name, key);

            object entity;
            bool result = _context.TryGetObjectByKey(ekey, out entity);
#if DEBUG
			Trace.WriteLine("Request for single entity with key " + key.ToString() + " from " + this._objectSet.EntitySet.Name);
#endif

			return entity as T;
		}

		/// <summary>
		/// Queries all of the entities
		/// </summary>
		public IQueryable<T> All()
		{
#if DEBUG
			Trace.WriteLine("Returing all from data provider (" + this._objectSet.EntitySet.Name + ")");
#endif
			return _objectSet;
		}

		public IQueryable<T> All(Expression<Func<T, bool>> predicate)
		{
			var query = _objectSet.Where(predicate);

#if DEBUG
			Trace.WriteLine("Returing all from data provider, with predicate (" + this._objectSet.EntitySet.Name + "):");
			Trace.WriteLine((query as ObjectQuery<T>).ToTraceString());
#endif

			return query;
		}

        /// <summary>
        /// Gets all the data, performing preload operations using the specified expression list.
        /// </summary>
        /// <param name="includes">The include expressions</param>
        public IQueryable<T> AllWithIncludes(params Expression<Func<T, object>>[] includes)
        {
            ObjectQuery<T> query = _objectSet;
            var exprParser = new ExpressionParser<T, object>();

            foreach (var item in includes)
            {
                string propertyPath = exprParser.GetPropertyPathFromExpression(item);

                query = query.Include(propertyPath);
            }

            return query;
        }

        /// <summary>
        /// Gets all the data, performing preload operations using the specified expression list. The data is filtered using the supplied predicate.
        /// </summary>
        /// <param name="predicate">A filtering predicate to perform on the resulting data set.</param>
        /// <param name="includes">The include expressions</param>
        public IQueryable<T> AllWithIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            ObjectQuery<T> query = _objectSet.Where(predicate) as ObjectQuery<T>;
            var exprParser = new ExpressionParser<T, object>();

            foreach (var item in includes)
            {
                string propertyPath = exprParser.GetPropertyPathFromExpression(item);

                query = query.Include(propertyPath);
            }

            return query;
        }

		/// <summary>
		/// Inserts the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void Insert(T entity)
		{
			_objectSet.AddObject(entity);
		}

		/// <summary>
		/// Updates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void Update(T entity)
		{
			ObjectStateEntry entry;

			bool result = _context.ObjectStateManager.TryGetObjectStateEntry(entity, out entry);

			if (!result || entry.State == EntityState.Detached)
			{
				this.Attach(entity);
			}

			_context.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
		}

		/// <summary>
		/// Deletes the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void Delete(T entity)
		{
			_context.DeleteObject(entity);
		}

		/// <summary>
		/// Saves the changes.
		/// </summary>
		public void SaveChanges()
		{
#if DEBUG
			Trace.WriteLine("Saving provider changes for " + this._objectSet.EntitySet.Name);
#endif
			_context.SaveChanges();
		}

		/// <summary>
		/// Attaches the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="state">The state.</param>
		public void Attach(T entity, State state = State.Unchanged)
		{
			_objectSet.Attach(entity);

			switch (state)
			{
				case State.New:
					{
						_context.ObjectStateManager.ChangeObjectState(entity, EntityState.Added);
					} break;

				case State.Modified:
					{
						_context.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
					} break;
			}
		}

		/// <summary>
		/// Detaches the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void Detach(T entity)
		{
			ObjectStateEntry entry;

			if (_context.ObjectStateManager.TryGetObjectStateEntry(entity, out entry))
			{
				if (entry.State != EntityState.Detached)
					_context.Detach(entity);
			}
		}

		/// <summary>
		/// Gets the object key.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public object GetObjectKey(T entity)
		{
			return entity.EntityKey.EntityKeyValues.First().Value;
		}

		/// <summary>
		/// Builds the fully qualified name of the set
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <param name="context">The context.</param>
		private string BuildFQName(string tableName, ObjectContext context)
		{
			return string.Format("{0}.{1}", context.DefaultContainerName, tableName);
		}

        #region Not implemented
        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
