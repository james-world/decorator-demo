using System.Data.Entity;
using System.Transactions;
using NUnit.Framework;

namespace DecoratorDemo
{
    public class CustomerSqlRepositoryIntegrationTests
    {
        [OneTimeSetUp]
        public void SetupDatabase()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<DecoratorDemoDbContext>());
            var context = new DecoratorDemoDbContext();
            context.Database.Initialize(false);
        }

        [Test]
        public void SaveAndRetrieveCustomer()
        {
            using (new TransactionScope())
            {
                var customer = new Customer(1, "James");
                ICustomerRepository repository = new CustomerSqlRepository(new DecoratorDemoDbContext());
                repository.Add(customer);
                var result = repository.GetById(1);

                Assert.AreEqual(customer.CustomerId, result.CustomerId);
                Assert.AreEqual(customer.Name, result.Name);
            }

        }
    }
}
