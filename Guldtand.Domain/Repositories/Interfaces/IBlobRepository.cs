using System.IO;
using System.Threading.Tasks;

namespace Guldtand.Domain.Repositories
{
    public interface IBlobRepository
    {
        Task<string> UploadImageAsBlobAsync(Stream stream, string filename);
    }
}