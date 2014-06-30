using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Kilo.Data.Azure
{
    public class BlobStorageRepository : IBlobRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="publicAccess">if set to <c>true</c> [public access].</param>
        public BlobStorageRepository(string connectionString, string containerName, bool publicAccess = false)
            : this(new StorageContext(connectionString), containerName, publicAccess)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageRepository" /> class.
        /// </summary>
        /// <param name="context">The storage context, describing how to connect to blob storage</param>
        /// <param name="containerName">Name of the blob container.</param>
        /// <param name="publicAccess">if set to <c>true</c>, a public access container is created.</param>
        public BlobStorageRepository(StorageContext context, string containerName, bool publicAccess = false)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (string.IsNullOrWhiteSpace(containerName))
            {
                throw new ArgumentException("containerName cannot be empty", "containerName");
            }

            this.StorageContext = context;
            this.BlobContainer = context.BlobClient.GetContainerReference(containerName);
            this.BlobContainer.CreateIfNotExists();

            if (publicAccess)
            {
                this.BlobContainer.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }
        }

        /// <summary>
        /// Gets or sets the storage context.
        /// </summary>
        public StorageContext StorageContext { get; private set; }

        /// <summary>
        /// Gets or sets the BLOB container.
        /// </summary>
        public CloudBlobContainer BlobContainer { get; private set; }

        /// <summary>
        /// Uploads the blob data.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Mime type of the content.</param>
        public void UploadBlobData(string name, Stream data, string contentType)
        {
            var block = this.BlobContainer.GetBlockBlobReference(name);
            block.Properties.ContentType = contentType;

            block.UploadFromStream(data);
        }

        /// <summary>
        /// Uploads the blob data asynchronously
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Mime type of the content.</param>
        public Task UploadBlobDataAsync(string name, Stream data, string contentType)
        {
            var block = this.BlobContainer.GetBlockBlobReference(name);
            block.Properties.ContentType = contentType;

            return block.UploadFromStreamAsync(data);
        }

        /// <summary>
        /// Gets the blob data.
        /// </summary>
        /// <param name="name">The name.</param>
        public Stream GetBlobData(string name)
        {
            var block = this.BlobContainer.GetBlockBlobReference(name);
            var dataStream = new MemoryStream();

            block.DownloadToStream(dataStream);
            dataStream.Position = 0;
            
            return dataStream;
        }

        /// <summary>
        /// Gets the blob data asyncronously
        /// </summary>
        /// <param name="name">The name.</param>
        public Task<Stream> GetBlobDataAsync(string name)
        {
            var block = this.BlobContainer.GetBlockBlobReference(name);
            var dataStream = new MemoryStream();

            return block.DownloadToStreamAsync(dataStream)
                .ContinueWith<Stream>(t =>
                {
                    dataStream.Position = 0;
                    return dataStream;
                });
        }

        /// <summary>
        /// Gets the url to download a particular blob
        /// </summary>
        public Uri GetBlobUrl(string filename)
        {
            var block = this.BlobContainer.GetBlockBlobReference(filename);

            return block.Uri;
        }

        /// <summary>
        /// Deletes the blob with the specified filename
        /// </summary>
        public void DeleteBlob(string filename)
        {
            var block = this.BlobContainer.GetBlockBlobReference(filename);

            block.DeleteIfExists();
        }
    }
}
