using System.Collections.Generic;
using Guldtand.Domain.Models;

namespace Guldtand.Domain.Repositories
{
    public interface IEmployeeRepository
    {
        List<IEmployee> GetAllEmployees();
    }
}