using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System.Diagnostics;

namespace Kilo.Data.Azure
{
    /// <summary>
    /// A delegate for events which are raised during the commit phase.
    /// </summary>
    /// <typeparam name="T">The type of the entity being commited</typeparam>
    /// <param name="context">Contextual information about the commit operation</param>
    public delegate void CommitEvent<T>(CommitContext<T> context) where T : class, ITableEntity, new();

    public class TableStorageRepository<T> : IRepository<T, TableStorageKey>,  IWriteableRepository<T>
        where T : class, ITableEntity, new()
    {
        private UnitOfWorkContainer<T> _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableStorageRepository{TSource}" /> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public TableStorageRepository(StorageContext context, string tableName)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException("tableName cannot be empty", "tableName");
            }

            this.TableName = tableName;
            this.Context = context;

            if (this.Context != null)
            {
                this.Table = this.Context.TableClient.GetTableReference(this.TableName);
                this.Table.CreateIfNotExists();
            }

            this.ResetUnitOfWork();
        }

        /// <summary>
        /// Occurs when an entity is being inserted
        /// </summary>
        public event CommitEvent<T> EntityInserting;

        /// <summary>
        /// Occurs when an entity is being updated
        /// </summary>
        public event CommitEvent<T> EntityUpdating;

        /// <summary>
        /// Occurs when an entity is being deleted
        /// </summary>
        public event CommitEvent<T> EntityDeleting;

        /// <summary>
        /// Occurs when a batch has been commited
        /// </summary>
        public event EventHandler BatchCommitted;

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        protected string TableName { get; private set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        protected StorageContext Context { get; private set; }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        protected CloudTable Table { get; private set; }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Insert(T entity)
        {
            this._uow.Inserts.Add(entity);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Update(T entity)
        {
            this._uow.Updates.Add(entity);
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete(T entity)
        {
            this._uow.Deletes.Add(entity);
        }

        /// <summary>
        /// Commits the operations which are currently in the unit of work
        /// </summary>
        public void Commit()
        {
            var batch = new TableBatchOperation();

            this._uow.Inserts.ForEach(entity =>
            {
                if (this.EntityInserting != null)
                {
                    this.EntityInserting(new CommitContext<T>(entity, batch));
                }

                batch.InsertOrMerge(entity);
            });

            this._uow.Updates.ForEach(entity =>
            {
                if (this.EntityUpdating != null)
                {
                    this.EntityUpdating(new CommitContext<T>(entity, batch));
                }

                batch.Merge(entity);
            });

            this._uow.Deletes.ForEach(entity =>
            {
                if (this.EntityDeleting != null)
                {
                    this.EntityDeleting(new CommitContext<T>(entity, batch));
                }

                batch.Delete(entity);
            });

            this.Table.ExecuteBatch(batch);

            if (this.BatchCommitted != null)
            {
                this.BatchCommitted(this, new EventArgs());
            }

            this.ResetUnitOfWork();
        }

        public T Single(TableStorageKey key)
        {
            IQueryable<T> query = this.Table.CreateQuery<T>()
                .Where(t => t.PartitionKey == key.PartitionKey)
                .Where(t => t.RowKey == key.RowKey);

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Gets entities based on a predicate filter.
        /// </summary>
        /// <param name="partitionKey">The partition key to filter on. A filter for partitionKey is automatically included by default.</param>
        /// <param name="predicates">The predicates to include in the filter.</param>
        public IQueryable<T> Query(params Expression<Func<T, bool>>[] predicates)
        {
            IQueryable<T> query = this.Table.CreateQuery<T>();

            if (predicates != null)
            {
                query = predicates.Aggregate(query, (current, pred) => current.Where(pred));
            }

            return query;
        }

        /// <summary>
        /// Performs a query using the specified resolver.
        /// </summary>
        /// <param name="resolver">The resolver to use when performing the query</param>
        /// <param name="predicates">The set of predicates to apply to the query</param>
        public IQueryable<T> QueryWithResolver(EntityResolver<T> resolver, params Expression<Func<T, bool>>[] predicates)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException("resolver");
            }

            IQueryable<T> query = this.Table.CreateQuery<T>().Resolve(resolver);

            if (predicates != null)
            {
                query = predicates.Aggregate(query, (current, pred) => current.Where(pred));
            }

            return query;
        }
        
        /// <summary>
        /// Resets the unit of work.
        /// </summary>
        protected virtual void ResetUnitOfWork()
        {
            this._uow = new UnitOfWorkContainer<T>();
        }

        /// <summary>
        /// Rolls back the current pending changes.
        /// </summary>
        public void Rollback()
        {
            this.ResetUnitOfWork();
        }

        public void Attach(T entity, State state = State.Unchanged)
        {
        }

        public void Detach(T entity)
        {
        }
    }
}
