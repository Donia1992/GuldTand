using Guldtand.Domain.Helpers;
using Guldtand.Domain.Models;
using Microsoft.Extensions.Options;

namespace GuldtandApi.Helpers
{
    public interface IJWTHelper
    {
        string GenerateTokenString(UserDTO user, string roleName, IOptions<AppSettings> _appSettings);
    }
}