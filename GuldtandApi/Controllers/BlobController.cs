using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Guldtand.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

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

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAllBlobsForOneCustomerAsync(string id)
        {
            try
            {
                var result = await _blobService.GetAllBlobsForOneCustomerAsync(id);
                return (Ok(result));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error caught in {nameof(BlobController)}, details: {exception.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> PostBlobToCustomerFolderAsync([FromForm] IFormFile file, string id)
        {
            try
            {
                if (file == null)
                    throw new ArgumentNullException($"Parameter {nameof(file)} cannot be null");

                var stream = file.OpenReadStream();
                var name = file.FileName;

                var result = await _blobService.UploadBlobToCustomerDirectoryAsync(stream, name, id);
                return (Ok(result));
            }
            catch (ArgumentNullException exception)
            {
                _logger.LogError($"Error caught in {nameof(BlobController)}, details: {exception.Message}");
                return StatusCode((int) HttpStatusCode.BadRequest, exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error caught in {nameof(BlobController)}, details: {exception.Message}");
                return StatusCode((int) HttpStatusCode.InternalServerError, exception.Message);
            }
        }
    }
}