using System.Linq;
using Guldtand.Data.Entities;

namespace Guldtand.Domain.Helpers
{
    public static class UserQueryObjects
    {
        public static IQueryable<User> Page(this IQueryable<User> @this, int pageNr, int n = 50)
        {
            return @this.Skip(n * (pageNr - 1)).Take(n);
        }
    }
}

