using Guldtand.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Guldtand.Domain.Services
{
    public interface IUserService
    {
        Task<(UserDTO, string)> AuthenticateAsync(string username, string password);
        Task<UserDTO> CreateAsync(UserDTO user, string password);
        UserDTO GetById(int id);
        IEnumerable<UserDTO> GetAll();
        void Update(UserDTO userDto, string password = null);
        void Delete(int id);
    }
}