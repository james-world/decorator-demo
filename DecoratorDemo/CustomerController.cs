namespace DecoratorDemo
{
    public class CustomerController
    {
        private ICustomerRepository customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public Customer Get(int customerId)
        {
            return customerRepository.GetById(customerId);
        }

        public void Post(Customer customer)
        {
            customerRepository.Add(customer);
        }
    }
}