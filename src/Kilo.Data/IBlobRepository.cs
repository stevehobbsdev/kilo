using System;
using System.IO;
using System.Threading.Tasks;

namespace Kilo.Data.Azure
{
    public interface IBlobRepository
    {
        /// <summary>
        /// Deletes the BLOB.
        /// </summary>
        /// <param name="filename">The filename.</param>
        void DeleteBlob(string filename);

        /// <summary>
        /// Gets the BLOB data.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Stream GetBlobData(string name);

        /// <summary>
        /// Gets the blob data asyncronously
        /// </summary>
        /// <param name="name">The name.</param>
        Task<Stream> GetBlobDataAsync(string name);

        /// <summary>
        /// Gets the blob URL.
        /// </summary>
        /// <param name="filename">The filename.</param>
        Uri GetBlobUrl(string filename);

        /// <summary>
        /// Uploads the BLOB data.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        void UploadBlobData(string name, Stream data, string contentType);

        /// <summary>
        /// Uploads the blob data asynchronously
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Mime type of the content.</param>
        Task UploadBlobDataAsync(string name, Stream data, string contentType);
    }
}
