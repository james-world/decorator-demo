using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace DecoratorDemo
{
    public class DecoratorDemoDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().Property(c => c.CustomerId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            base.OnModelCreating(modelBuilder);
        }
    }
}