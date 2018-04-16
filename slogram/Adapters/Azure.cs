using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace slogram.Adapters
{
    public static class Azure
    {
        public static CloudBlockBlob Blob(string blobpath)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("storageconnectionstring"));
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            var container = cloudBlobClient.GetContainerReference("slogramblobcontainer");
            return container.GetBlockBlobReference(blobpath);
        }
    }
}
