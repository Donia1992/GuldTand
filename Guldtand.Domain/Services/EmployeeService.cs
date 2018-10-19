using Guldtand.Domain.Models;
using Guldtand.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Guldtand.Domain.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<List<IEmployee>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            return employees;
        }
    }
}
