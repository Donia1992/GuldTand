using System.Collections.Generic;
using System.Threading.Tasks;
using Guldtand.Domain.Models;

namespace Guldtand.Domain.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<IEmployee>> GetAllEmployeesAsync();
    }
}