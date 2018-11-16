using System;

namespace Guldtand.Domain.Helpers
{
    public interface IDateTimeProvider
    {
        bool IsValidDate(string pid);
        DateTime Today();
    }
}