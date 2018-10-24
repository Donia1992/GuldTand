using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Guldtand.Domain.Services;
using Microsoft.Extensions.Logging;

namespace GuldtandApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly IBlobService _blobService;
        private readonly ILogger<BlobController> _logger;

        public BlobController(ILogger<BlobController> logger, IBlobService blobService)
        {
            _logger = logger;
            _blobService = blobService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> PostXrayBlob([FromForm] IFormFile file)
        {
            var stream = file.OpenReadStream();
            var name = file.FileName;

            //return Ok(name);
            return Ok(await _blobService.UploadXrayBlobAsync(stream, name));
        }
    }
}