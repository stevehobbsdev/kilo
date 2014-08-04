using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace Kilo.Data.Azure
{
    public class UnitOfWorkContainer<T>
    {
        /// <summary>
        /// Gets the inserts.
        /// </summary>
        public List<T> Inserts { get; private set; }

        /// <summary>
        /// Gets or sets the updates.
        /// </summary>
        public List<T> Updates { get; private set; }

        /// <summary>
        /// Gets the deletes.
        /// </summary>
        public List<T> Deletes { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkContainer" /> class.
        /// </summary>
        public UnitOfWorkContainer()
        {
            this.Inserts = new List<T>();
            this.Deletes = new List<T>();
            this.Updates = new List<T>();
        }
    }

    public static class UnitOfWorkExtensions
    {
        public static void ApplyToBatch<T>(this UnitOfWorkContainer<T> unitOfWork, TableBatchOperation batchOperation)
            where T : TableEntity
        {
            unitOfWork.Deletes.ForEach(e =>
            {
                batchOperation.Delete(e);
            });

            unitOfWork.Inserts.ForEach(e =>
            {
                batchOperation.InsertOrMerge(e);
            });

            unitOfWork.Updates.ForEach(e =>
            {
                batchOperation.Merge(e);
            });
        }

        public static void ApplyToBatch<TSource>(this UnitOfWorkContainer<TSource> unitOfWork, TableBatchOperation batchOperation, Func<TSource, ITableEntity> transform)
        {
            if (transform == null)
            {
                throw new ArgumentNullException("transform");
            }

            unitOfWork.Deletes.ForEach(e =>
            {
                batchOperation.Delete(transform(e));
            });

            unitOfWork.Inserts.ForEach(e =>
            {
                batchOperation.InsertOrMerge(transform(e));
            });

            unitOfWork.Updates.ForEach(e =>
            {
                batchOperation.Merge(transform(e));
            });
        }
    }
}
