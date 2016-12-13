using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace DecoratorDemo
{
    public class CachingCustomerRepositoryTests
    {
        private ICustomerRepository repository;
        private ICustomerRepository cache;
        private TimeSpan cacheEntryLifetime;

        [SetUp]
        public void CreateCachingRepository()
        {
            repository = Substitute.For<ICustomerRepository>();
            cacheEntryLifetime = TimeSpan.FromMilliseconds(100);
            cache = new CachingCustomerRepository(repository, cacheEntryLifetime);
            
        }


        [Test]
        public void ImplementsRepositoryInterface()
        {
            Assert.IsInstanceOf<ICustomerRepository>(cache);
        }

        [Test]
        public void CallsUnderlyingRepositoryToGetCustomers()
        {
            var expected = new Customer(1, "James");
            repository.GetById(1).Returns(expected);

            var result = cache.GetById(1);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CallsUnderlyingRepositoryToAddCustomers()
        {
            var customer = new Customer(1, "James");
            cache.Add(customer);

            repository.Received(1).Add(customer);
        }

        [Test]
        public void SecondCallToGetForSameIdDoesNotCallUnderlyingRepository()
        {
            int customerId = 1;
            var expected = new Customer(customerId, "James");
            repository.GetById(customerId).Returns(expected);

            cache.GetById(customerId);
            cache.GetById(customerId);

            repository.Received(1).GetById(customerId);
        }

        [Test]
        public async Task SecondCallToGetForSameIdDoesCallsUnderlyingRepositoryIfCacheTimeHasElapsed()
        {
            int customerId = 1;
            var expected = new Customer(customerId, "James");
            repository.GetById(customerId).Returns(expected);

            cache.GetById(customerId);
            await Task.Delay(TimeSpan.FromMilliseconds(200));
            cache.GetById(customerId);

            repository.Received(2).GetById(customerId);
        }
    }

    public class CachingCustomerRepository : ICustomerRepository
    {
        private readonly ICustomerRepository repository;
        private readonly Dictionary<int, Customer> cache = new Dictionary<int, Customer>();
        private readonly TimeSpan timeSpan;

        public CachingCustomerRepository(ICustomerRepository repository, TimeSpan cacheEntryLifetime)
        {
            timeSpan = cacheEntryLifetime;
            this.repository = repository;
        }

        public void Add(Customer customer)
        {
            repository.Add(customer);
        }

        public Customer GetById(int customerId)
        {
            if (cache.ContainsKey(customerId))
                return cache[customerId];

            var customer = repository.GetById(customerId);
            cache.Add(customerId, customer);
            return customer;
        }
    }
}