namespace DecoratorDemo
{
    public interface ICustomerRepository
    {
        void Add(Customer customer);
        Customer GetById(int customerId);
    }
}