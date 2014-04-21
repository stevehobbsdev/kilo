using System;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace Kilo.Data.Azure
{
    public class StorageContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageContext" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public StorageContext(string connectionString = null)
        {
            this.Account = GetCloudStorageAccount(connectionString);
            this.TableClient = this.Account.CreateCloudTableClient();
            this.BlobClient = this.Account.CreateCloudBlobClient();
        }

        /// <summary>
        /// Gets or sets the table client.
        /// </summary>
        public CloudTableClient TableClient { get; set; }

        /// <summary>
        /// Gets or sets the BLOB client.
        /// </summary>
        public CloudBlobClient BlobClient { get; set; }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        public CloudStorageAccount Account { get; set; }

        /// <summary>
        /// Gets the base address.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        private static Uri GetBaseAddress(string connectionString)
        {
            return new Uri(StorageContext.GetCloudStorageAccount(connectionString).TableEndpoint.ToString());
        }

        /// <summary>
        /// Gets the cloud credentials.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        private static StorageCredentials GetCloudCredentials(string connectionString)
        {
            return StorageContext.GetCloudStorageAccount(connectionString).Credentials;
        }

        /// <summary>
        /// Gets the cloud storage account.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        private static CloudStorageAccount GetCloudStorageAccount(string connectionString) 
        {
            if(string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("A connection string must be supplied", "connectionString");
            }

            var cloudStorageAccount = CloudStorageAccount.Parse(connectionString);

            return cloudStorageAccount;
        }
    }
}
