using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Guldtand.Domain.Models.DTOs;

namespace Guldtand.Domain.Services
{
    public interface IBlobService
    {
        Task<BlobDTO> UploadBlobAsync(Stream stream, string fileName);
        Task<List<BlobDTO>> GetAllBlobsForOneCustomerAsync(string customerId);
        Task<BlobDTO> UploadBlobToCustomerDirectoryAsync(Stream stream, string fileName, string customerId);
    }
}