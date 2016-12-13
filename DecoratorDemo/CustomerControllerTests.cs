using NSubstitute;
using NUnit.Framework;

namespace DecoratorDemo
{
    public class CustomerControllerTests
    {
        [Test]
        public void GetCustomerById()
        {
            int customerId = 1;
            var repository = Substitute.For<ICustomerRepository>();
            var expectedCustomer = new Customer(customerId, "James");
            repository.GetById(customerId).Returns(expectedCustomer);

            var customerController = new CustomerController(repository);
            Customer customer = customerController.Get(customerId);

            Assert.AreEqual(expectedCustomer, customer);
        }

        [Test]
        public void PersistCustomerDetails()
        {
            var customer = new Customer(1, "James");
            var repository = Substitute.For<ICustomerRepository>();
            var customerController = new CustomerController(repository);
            customerController.Post(customer);

            repository.Received(1).Add(customer);
        }
    }
}
