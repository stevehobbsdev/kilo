using System;
namespace Kilo.Data.Azure
{
    interface IBlobStorageRepository
    {
        void DeleteBlob(string filename);
        System.IO.Stream GetBlobData(string name);
        Uri GetBlobUrl(string filename);
        void UploadBlobData(string name, System.IO.Stream data, string contentType);
    }
}
