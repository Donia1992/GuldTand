using Guldtand.Data.Entities;
using System.Threading.Tasks;

namespace Guldtand.Domain.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        User Create(User user, string password);
    }
}