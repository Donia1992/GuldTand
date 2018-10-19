using Microsoft.EntityFrameworkCore;

namespace Guldtand.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Employee> Employees { get; set; }
    }
}
