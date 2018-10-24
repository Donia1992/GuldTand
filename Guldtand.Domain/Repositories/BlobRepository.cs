using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;


namespace Guldtand.Domain.Repositories
{
    public class BlobRepository : IBlobRepository
    {
        private readonly IOptions<BlobSettings> _configuration;
        public BlobRepository(IOptions<BlobSettings> configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> UploadImageAsBlobAsync(Stream stream, string filename)
        {
            StorageCredentials storageCredentials = new StorageCredentials(_configuration.Value.GuldName, _configuration.Value.GuldKey);
            CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, useHttps: true);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference("xrays");

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);

            await blockBlob.UploadFromStreamAsync(stream);

            stream.Dispose();
            return blockBlob?.Uri.ToString();
        }
    }
}
