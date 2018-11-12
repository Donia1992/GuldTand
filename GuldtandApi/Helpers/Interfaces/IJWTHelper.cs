using Guldtand.Data.Entities;
using Guldtand.Domain.Helpers;
using Microsoft.Extensions.Options;

namespace GuldtandApi.Helpers
{
    public interface IJWTHelper
    {
        string GenerateTokenString(User user, IOptions<AppSettings> _appSettings);
    }
}