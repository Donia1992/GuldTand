using System.Collections.Generic;
using System.Threading.Tasks;
using Guldtand.Domain.Models;

namespace Guldtand.Domain.Services
{
    public interface ICustomerService
    {
        Task<CustomerDTO> RegisterAsync(CustomerDTO customer);
        void Delete(int id);
        IEnumerable<CustomerDTO> GetAll();
        CustomerDTO GetById(int id);
        void Update(CustomerDTO customerDto);
    }
}