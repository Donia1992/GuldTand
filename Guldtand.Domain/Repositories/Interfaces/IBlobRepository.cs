using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Guldtand.Domain.Repositories
{
    public interface IBlobRepository
    {
        Task<List<ValueTuple<string, string>>> GetAllBlobsForOneCustomerAsync(string customerId);
        Task<ValueTuple<string, string>> UploadBlobToCustomerFolderAsync(Stream stream, string fileName, string customerId);
    }
}