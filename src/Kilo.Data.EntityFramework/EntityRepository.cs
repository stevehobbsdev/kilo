using System;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Kilo.Expressions;

namespace Kilo.Data.EntityFramework
{
	public class EntityRepository<T, TKey> : IEntityRepository<T, TKey>
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
		public T Single(TKey key)
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

		public IQueryable<T> Query(params Expression<Func<T, bool>> [] predicates)
		{
            IQueryable<T> query = this._objectSet;

            if(predicates != null)
            {
                query = predicates.Aggregate(query, (current, predicate) => current.Where(predicate));
            }

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
            IQueryable<T> query = _objectSet;

            if(includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
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
            IQueryable<T> query = _objectSet.Where(predicate);

            if(includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
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

        public void Commit()
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
        public void Rollback()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
