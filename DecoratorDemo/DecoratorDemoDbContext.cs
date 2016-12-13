using System.Data.Entity;

namespace DecoratorDemo
{
    public class DecoratorDemoDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
    }
}