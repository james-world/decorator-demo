using System;
using System.Data.Entity;
using System.Reflection;
using Autofac;
using NUnit.Framework;

namespace DecoratorDemo
{
    public class CustomerControllerEndToEndTests
    {
        [OneTimeSetUp]
        public void SetupDatabase()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<DecoratorDemoDbContext>());
            var context = new DecoratorDemoDbContext();
            context.Database.Initialize(false);
        }


        [Test]
        public void CustomerSavedAndRetrieved()
        {
            var cacheTimeout = TimeSpan.FromSeconds(30);

            var builder = new ContainerBuilder();
            builder.RegisterType<DecoratorDemoDbContext>();
            builder.RegisterType<CustomerSqlRepository>().Named<ICustomerRepository>("CustomerRepository");
            builder.RegisterDecorator<ICustomerRepository>((c, inner) =>
                new CachingCustomerRepository(inner, cacheTimeout),
                fromKey: "CustomerRepository");
            builder.RegisterType<CustomerController>();

            var container = builder.Build();

            var controller = container.Resolve<CustomerController>();

            controller.Post(new Customer(37,"Dave"));

            var customer = controller.Get(37);

            Assert.AreEqual(37, customer.CustomerId);
            Assert.AreEqual("Dave", customer.Name);

        }
    }
}