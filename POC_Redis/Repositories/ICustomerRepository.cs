using POC_Redis.Models;

namespace POC_Redis.Repositories
{
    public interface ICustomerRepository
    {
        public Task<IEnumerable<Customer>> GetCustomers();

        public Task<Customer> GetCustomer(Guid id);        
    }
}
