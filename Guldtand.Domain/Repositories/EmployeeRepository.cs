using Guldtand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guldtand.Domain.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public async Task<List<IEmployee>> GetAllEmployeesAsync()
        {
            using (var db = new Db())
            {
                return await Task.Run(() => db.Employees.ToList());
            }
        }
    }

    public class Db : IDisposable
    {
        public List<IEmployee> Employees => new List<IEmployee>
        {
            //new Dentist { Uid = 0001, Password = "password", Name = "Russell Collins" },
            //new Dentist { Uid = 0002, Password = "qwerty", Name = "Samantha Jensen" },
            //new Administrator { Uid = 0003, Password = "admin", Name = "Ashley Scott" }
        };

        public void Dispose()
        {
            Console.WriteLine("Disposing");
        }
    }
}

