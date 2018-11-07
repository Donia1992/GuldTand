using Guldtand.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Guldtand.Domain.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        User Create(User user, string password);
        User GetById(int id);
        IEnumerable<User> GetAll();
    }
}