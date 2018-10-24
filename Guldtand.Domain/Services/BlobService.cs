using Guldtand.Domain.Models.DTOs;
using Guldtand.Domain.Repositories;
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

        public async Task<XrayBlobDTO> UploadXrayBlobAsync(Stream stream, string fileName)
        {
            var blobURI = await _blobRepository.UploadImageAsBlobAsync(stream, fileName);
            var blobDTO = new XrayBlobDTO(blobURI);
            return blobDTO;
        }
    }
}
