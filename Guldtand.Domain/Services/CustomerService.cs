﻿using AutoMapper;
using Guldtand.Data;
using Guldtand.Data.Entities;
using Guldtand.Domain.Helpers;
using Guldtand.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guldtand.Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CustomerService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CustomerDTO> RegisterAsync(CustomerDTO customerDto)
        {
            if (!customerDto.PIDNumber.LuhnCheck())
                throw new AppException($"PID number {customerDto.PIDNumber} is invalid.");
            
            if (_context.Customers.Any(x => x.PIDNumber == customerDto.PIDNumber))
                throw new AppException($"A customer with PID {customerDto.PIDNumber} already exists in the database.");

            if (_context.Customers.Any(x => x.Email == customerDto.Email))
                throw new AppException($"A customer with email {customerDto.Email} already exists in the database.");

            var customer = _mapper.Map<Customer>(customerDto);

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return customerDto;
        }

        public IEnumerable<CustomerDTO> GetAll()
        {
            var customers = _context.Customers;
            var customerDtos = _mapper.Map<IEnumerable<CustomerDTO>>(customers);
            return customerDtos;
        }

        public CustomerDTO GetById(int id)
        {
            var customer = _context.Customers.Find(id);
            return _mapper.Map<CustomerDTO>(customer);
        }

        public void Update(CustomerDTO customerDto)
        {
            var customer = _context.Customers.Find(customerDto.Id);

            if (customer == null)
                throw new AppException($"Customer with id {customerDto.Id} not found.");

            customer.FirstName = customerDto.FirstName ?? customer.FirstName;
            customer.LastName = customerDto.LastName ?? customer.LastName;
            customer.Phone = customerDto.Phone ?? customer.Phone;
            customer.Email = customerDto.Email ?? customer.Email;
            customer.Street = customerDto.Street ?? customer.Street;
            customer.Zip = customerDto.Zip ?? customer.Zip;
            customer.City = customerDto.City ?? customer.City;
            customer.HasInsurance = (customer.HasInsurance == customerDto.HasInsurance) ? customer.HasInsurance : customerDto.HasInsurance;

            _context.Customers.Update(customer);
            _context.SaveChanges();
    }

        public void Delete(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
            else throw new AppException($"Customer with id {id} not found.");
        }
    }
}