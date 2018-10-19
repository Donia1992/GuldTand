using System;
using Microsoft.AspNetCore.Mvc;
using Guldtand.Domain.Repositories;
using System.Net;

namespace GuldtandApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        [Route("")]
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