using System.Collections.Generic;
using Guldtand.Domain.Models;

namespace Guldtand.Domain.Services
{
    public interface ICustomerService
    {
        CustomerDTO Create(CustomerDTO customer);
        IEnumerable<CustomerDTO> GetAll();
        CustomerDTO GetById(int id);
        void Update(CustomerDTO customerDto);
        void Delete(int id);
    }
}