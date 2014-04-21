using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Kilo.Data.Azure
{
    public class CommitContext<T> where T : ITableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommitContext{TTable}" /> class.
        /// </summary>
        /// <param name="tableEntity">The table entity.</param>
        /// <param name="operation">The operation.</param>
        /// <exception cref="System.ArgumentNullException">domainEntity</exception>
        public CommitContext(T tableEntity, TableBatchOperation operation)
        {
            if (tableEntity == null)
            {
                throw new ArgumentNullException("tableEntity");
            }

            if (operation == null)
            {
                throw new ArgumentNullException("operation");
            }

            this.Entity = tableEntity;
            this.Operation = operation;
        }

        /// <summary>
        /// Gets or sets the table entity.
        /// </summary>
        public T Entity { get; set; }

        /// <summary>
        /// Gets the batch operation.
        /// </summary>
        public TableBatchOperation Operation { get; private set; }
    }
}
