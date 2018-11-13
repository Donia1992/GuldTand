using Guldtand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guldtand.Domain.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public List<CustomerDTO> GetAllCustomers()
        {
            using (var db = new Db())
            {
                var customers = db.Customers.ToList();
                return customers;
            }
        }

        public class Db : IDisposable
        {
            public List<CustomerDTO> Customers => new List<CustomerDTO>
            {
                new CustomerDTO { Id = 0001, FirstName = "Dave", LastName = "Martin", PIDNumber = "785HNZ592" , Phone = "0205533449" , Email = "dave@work.com" , Street = "353 North Avenue" , Zip = "54YH3G" , City = "Seattle" , HasInsurance = true },
                new CustomerDTO { Id = 0001, FirstName = "Julia", LastName = "Chen", PIDNumber = "8933LNZ592" , Phone = "020121289" , Email = "julia@work.com" , Street = "2:ond Upper Street" , Zip = "22LH3G" , City = "Seattle" , HasInsurance = true },
                new CustomerDTO { Id = 0001, FirstName = "Mike", LastName = "Scott", PIDNumber = "231YYG592" , Phone = "020935422" , Email = "scott@work.com" , Street = "33 Green Close" , Zip = "131P3G" , City = "Seattle" , HasInsurance = true }
            };

            public void Dispose()
            {
                Console.WriteLine("Disposing");
            }
        }
    }
}


