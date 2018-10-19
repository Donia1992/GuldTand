using System;
using Microsoft.AspNetCore.Mvc;
using Guldtand.Domain.Repositories;
using System.Net;
using System.Threading.Tasks;

namespace GuldtandApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllEmployeesAsync()
        {
            try
            {
                var employeeList = await _employeeRepository.GetAllEmployeesAsync();
                return Ok(employeeList);
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