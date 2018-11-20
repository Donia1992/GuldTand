using System;
using System.Linq;
using Guldtand.Data.Entities;

namespace Guldtand.Domain.Helpers
{
    public static class ActivityQueryObjects
    {
        public static IQueryable<Activity> AsOf(this IQueryable<Activity> @this, DateTime date)
        {
            return @this.Where(x => x.Begin > date);
        }

        public static IQueryable<Activity> UserMatch(this IQueryable<Activity> @this, int id)
        {
            return @this.Where(x => x.UserId == id);
        }

        public static IQueryable<Activity> CustomerMatch(this IQueryable<Activity> @this, int id)
        {
            return @this.Where(x => x.CustomerId > id);
        }
    }
}