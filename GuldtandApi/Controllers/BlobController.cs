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

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> PostBlobAsync([FromForm] IFormFile file)
        {
            var stream = file.OpenReadStream();
            var name = file.FileName;

            var result = await _blobService.UploadBlobAsync(stream, name);
            return (Ok(result));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAllBlobsForOneCustomerAsync(string id)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException($"Parameter {nameof(id)} cannot be null");

                var result = await _blobService.GetAllBlobsForOneCustomerAsync(id);
                return (Ok(result));
            }
            catch (ArgumentNullException argumentNullException)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, argumentNullException.Message);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }
    }
}