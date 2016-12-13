using System;
using System.Runtime.Caching;

namespace DecoratorDemo
{
    public class CachingCustomerRepository : ICustomerRepository
    {
        private readonly ICustomerRepository repository;
        private readonly MemoryCache cache = new MemoryCache(typeof(CachingCustomerRepository).FullName);
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
            var customer = (Customer)cache.Get(customerId.ToString());
            if (customer != null)
                return customer;

            customer = repository.GetById(customerId);

            if (customer == null)
                return null;

            var expiry = DateTime.UtcNow.Add(timeSpan);
            var cacheEntry = new CacheItem(customerId.ToString(), customer);
            var cachePolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = new DateTimeOffset(expiry)
            };

            cache.Add(cacheEntry, cachePolicy);

            return customer;
        }
    }
}