namespace DecoratorDemo
{
    public class CustomerSqlRepository : ICustomerRepository
    {
        private readonly DecoratorDemoDbContext context;

        public CustomerSqlRepository(DecoratorDemoDbContext context)
        {
            this.context = context;
        }

        public void Add(Customer customer)
        {
            context.Customers.Add(customer);
            context.SaveChanges();
        }

        public Customer GetById(int customerId)
        {
            return context.Customers.Find(customerId);
        }
    }
}