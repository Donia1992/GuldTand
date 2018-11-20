using System;
using Microsoft.AspNetCore.Mvc;
using Guldtand.Domain.Repositories;
using System.Net;
using Guldtand.Domain.Helpers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Guldtand.Domain.Services;
using Guldtand.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace GuldtandApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Dentist,Receptionist")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(
            ICustomerRepository customerRepository,
            ICustomerService customerService,
            ILogger<CustomersController> logger
            )
        {
            _customerRepository = customerRepository;
            _customerService = customerService;
            _logger = logger;

        }

        [HttpPost]
        public IActionResult Register([FromBody] CustomerDTO customerDto)
        {
            try
            {
                _customerService.Create(customerDto);
                return Ok();
            }
            catch (AppException ex)
            {
                _logger.LogError($"Error caught in {nameof(CustomersController)}, details: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
            catch(Exception ex)
            {
                _logger.LogError($"Unhandled error in {nameof(CustomerService)}, details: {ex.InnerException.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var customers = _customerService.GetAll();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var customer = _customerService.GetById(id);
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CustomerDTO customerDto)
        {
            customerDto.Id = id;

            try
            {
                _customerService.Update(customerDto);
                return Ok();
            }
            catch (AppException ex)
            {
                _logger.LogError($"Error caught in {nameof(CustomersController)}, details: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _customerService.Delete(id);
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/old/dummyCustomerData")]
        public IActionResult GetAllCustomers()
        {
            try
            {
                var customerList = _customerRepository.GetAllCustomers();
                return Ok(customerList);
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: " + $"ArgumentException details: {exception.Message}");

                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: " + $"Exception details: {exception.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            finally
            {
                Console.WriteLine();
            }
        }
    }
}
