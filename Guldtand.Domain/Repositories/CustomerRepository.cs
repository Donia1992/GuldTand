using Guldtand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guldtand.Domain.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public List<ICustomer> GetAllCustomers()
        {
            using (var db = new Db())
            {
                var customers = db.Customers.ToList();
                return customers;
            }
        }

        public class Db : IDisposable
        {
            public List<ICustomer> Customers => new List<ICustomer>
            {
                new Person { Uid = 0001, Firstname = "Dave", Lastname = "Martin", Socialsecurity_number = "785HNZ592" , Phone = "0205533449" , Email = "dave@work.com" , Street = "353 North Avenue" , Zip = "54YH3G" , City = "Seattle" , Insurance = true },
                new Person { Uid = 0001, Firstname = "Julia", Lastname = "Chen", Socialsecurity_number = "8933LNZ592" , Phone = "020121289" , Email = "julia@work.com" , Street = "2:ond Upper Street" , Zip = "22LH3G" , City = "Seattle" , Insurance = true },
                new Person { Uid = 0001, Firstname = "Mike", Lastname = "Scott", Socialsecurity_number = "231YYG592" , Phone = "020935422" , Email = "scott@work.com" , Street = "33 Green Close" , Zip = "131P3G" , City = "Seattle" , Insurance = true }
            };

            public void Dispose()
            {
                Console.WriteLine("Disposing");
            }
        }
    }
}


