using System.IO;
using System.Threading.Tasks;
using Guldtand.Domain.Models.DTOs;

namespace Guldtand.Domain.Services
{
    public interface IBlobService
    {
        Task<XrayBlobDTO> UploadXrayBlobAsync(Stream stream, string fileName);
    }
}