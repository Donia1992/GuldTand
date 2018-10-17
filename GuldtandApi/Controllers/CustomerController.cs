using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GuldtandApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var response = string.Empty;

                return await Task.Run(() => Ok(response));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Unknown error in {nameof(CustomerController)}, details: {exception.Message}");
                return await Task.Run(() => StatusCode((int)HttpStatusCode.InternalServerError, "An unknown error occurred"));
            }
        }
    }
}