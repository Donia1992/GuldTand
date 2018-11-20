using System;

namespace Guldtand.Domain.Models
{
    public class ActivityDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public int TypeId { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
    }
}
