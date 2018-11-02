using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Guldtand.Domain.Repositories
{
    public class BlobRepository : IBlobRepository
    {
        private readonly IOptions<BlobSettings> _configuration;
        public BlobRepository(IOptions<BlobSettings> configuration)
        {
            _configuration = configuration;
        }

        public CloudBlobContainer GetContainer()
        {
            StorageCredentials storageCredentials = new StorageCredentials(_configuration.Value.GuldName, _configuration.Value.GuldKey);
            CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, useHttps: true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("xrays");
            return container;
        }

        public async Task<ValueTuple<string, string>> UploadImageAsBlobAsync(Stream stream, string filename)
        {
            CloudBlobContainer container = GetContainer();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);

            await blockBlob.UploadFromStreamAsync(stream);

            stream.Dispose();

            (string Name, string Url) blobInfo = new ValueTuple<string, string>(blockBlob.Name, blockBlob.Uri.ToString());

            return blobInfo;
        }

        public async Task<List<ValueTuple<string, string>>> GetAllBlobsForOneCustomerAsync(string customerId)
        {
            CloudBlobContainer container = GetContainer();
            CloudBlobDirectory directory = container.GetDirectoryReference($"{customerId}");

            var list = new List<ValueTuple<string, string>>();
            BlobContinuationToken token = null;
            do
            {
                BlobResultSegment resultSegment =
                    await directory.ListBlobsSegmentedAsync(token);
                token = resultSegment.ContinuationToken;
                
                foreach (IListBlobItem item in resultSegment.Results)
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    await blob.FetchAttributesAsync();
                    list.Add( 
                        new ValueTuple<string, string>(blob.Name.ToString().Substring(blob.Name.ToString().IndexOf("/") + 1), item.Uri.ToString()));
                }
            } while (token != null);

            return list;
        }

        public async Task<ValueTuple<string, string>> UploadBlobToCustomerFolderAsync(Stream stream, string filename, string customerId)
        {
            CloudBlobContainer container = GetContainer();
            CloudBlobDirectory directory = container.GetDirectoryReference($"{customerId}");

            CloudBlockBlob blockBlob = directory.GetBlockBlobReference(filename);

            await blockBlob.UploadFromStreamAsync(stream);
            stream.Dispose();

            (string Name, string Url) blobInfo = new ValueTuple<string, string>(blockBlob.Name, blockBlob.Uri.ToString());

            return blobInfo;
        }
    }
}
