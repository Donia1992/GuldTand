using Guldtand.Domain.Models.DTOs;
using Guldtand.Domain.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Guldtand.Domain.Services
{
    public class BlobService : IBlobService
    {
        private readonly IBlobRepository _blobRepository;

        public BlobService(IBlobRepository blobRepository)
        {
            _blobRepository = blobRepository;
        }

        public async Task<List<BlobDTO>> GetAllBlobsForOneCustomerAsync(string customerId)
        {
            var blobs = await _blobRepository.GetAllBlobsForOneCustomerAsync(customerId);
            List<BlobDTO> blobList = new List<BlobDTO>();

            foreach (var blob in blobs)
            {
                blobList.Add(new BlobDTO(blob.Item1, blob.Item2));
            }

            return blobList;
        }

        public async Task<BlobDTO> UploadBlobToCustomerDirectoryAsync(Stream stream, string fileName, string customerId)
        {

            var blobInfo = await _blobRepository.UploadBlobToCustomerFolderAsync(stream, fileName, customerId);
            var blobDTO = new BlobDTO(blobInfo.Item1, blobInfo.Item2);

            return blobDTO;
        }
    }
}
