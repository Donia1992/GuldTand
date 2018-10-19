using System.Collections.Generic;
using System.Threading.Tasks;
using Guldtand.Domain.Models;

namespace Guldtand.Domain.Services
{
    public interface IEmployeeService
    {
        Task<List<IEmployee>> GetAllEmployeesAsync();
    }
}