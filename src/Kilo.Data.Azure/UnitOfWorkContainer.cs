using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace Kilo.Data.Azure
{
    public enum UnitOfWorkEntryType
    {
        Insert = 0,
        Update = 1,
        Delete = 2
    }

    public class UnitOfWorkContainer<T>
    {
        internal class JournalEntry<T>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="JournalEntry`1"/> class.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="entity">The entity.</param>
            /// <param name="tick">The tick.</param>
            public JournalEntry(UnitOfWorkEntryType type, T entity, long tick)
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                this.Type = type;
                this.Entity = entity;
                this.Tick = tick;
            }

            /// <summary>
            /// Gets or sets the type.
            /// </summary>
            public UnitOfWorkEntryType Type { get; set; }

            /// <summary>
            /// Gets or sets the entity.
            /// </summary>
            public T Entity { get; set; }

            /// <summary>
            /// Gets the tick.
            /// </summary>
            public long Tick { get; private set; }
        }

        ///// <summary>
        ///// Gets the inserts.
        ///// </summary>
        //public List<T> Inserts { get; private set; }

        ///// <summary>
        ///// Gets or sets the updates.
        ///// </summary>
        //public List<T> Updates { get; private set; }

        ///// <summary>
        ///// Gets the deletes.
        ///// </summary>
        //public List<T> Deletes { get; private set; }

        private static long _tick = 1;
        private List<JournalEntry<T>> _journal;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkContainer" /> class.
        /// </summary>
        public UnitOfWorkContainer()
        {
            this.Reset();
        }

        public void Insert(T entity)
        {
            this._journal.Add(new JournalEntry<T>(UnitOfWorkEntryType.Insert, entity, _tick));
            UpdateTick();
        }

        public void Update(T entity)
        {
            this._journal.Add(new JournalEntry<T>(UnitOfWorkEntryType.Update, entity, _tick));
            UpdateTick();
        }

        public void Delete(T entity)
        {
            this._journal.Add(new JournalEntry<T>(UnitOfWorkEntryType.Delete, entity, _tick));
            UpdateTick();
        }

        internal IReadOnlyCollection<JournalEntry<T>> GetJournal()
        {
            return this._journal;
        }

        public void Reset()
        {
            this._journal = new List<JournalEntry<T>>();
        }

        private static void UpdateTick()
        {
            _tick = (_tick + 1) % long.MaxValue;
        }
    }

    //public static class UnitOfWorkExtensions
    //{
    //    public static void ApplyToBatch<T>(this UnitOfWorkContainer<T> unitOfWork, TableBatchOperation batchOperation)
    //        where T : TableEntity
    //    {
    //        unitOfWork.Deletes.ForEach(e =>
    //        {
    //            batchOperation.Delete(e);
    //        });

    //        unitOfWork.Inserts.ForEach(e =>
    //        {
    //            batchOperation.InsertOrMerge(e);
    //        });

    //        unitOfWork.Updates.ForEach(e =>
    //        {
    //            batchOperation.Merge(e);
    //        });
    //    }

    //    public static void ApplyToBatch<TSource>(this UnitOfWorkContainer<TSource> unitOfWork, TableBatchOperation batchOperation, Func<TSource, ITableEntity> transform)
    //    {
    //        if (transform == null)
    //        {
    //            throw new ArgumentNullException("transform");
    //        }

    //        unitOfWork.Deletes.ForEach(e =>
    //        {
    //            batchOperation.Delete(transform(e));
    //        });

    //        unitOfWork.Inserts.ForEach(e =>
    //        {
    //            batchOperation.InsertOrMerge(transform(e));
    //        });

    //        unitOfWork.Updates.ForEach(e =>
    //        {
    //            batchOperation.Merge(transform(e));
    //        });
    //    }
    //}
}
