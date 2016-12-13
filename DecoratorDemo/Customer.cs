namespace DecoratorDemo
{
    public class Customer
    {
        private Customer()
        {
            /* For Entity Framework */
        }

        public Customer(int customerId, string name)
        {
            CustomerId = customerId;
            Name = name;
        }

        public int CustomerId { get; set; }
        public string Name { get; set; }
    }
}