using POC_Redis.Models;

namespace POC_Redis.Services
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomer(Guid id);
        Task<IEnumerable<Customer>> GetCustomers();
        Task<IEnumerable<Customer>> GetExternalCustomers();
    }
}
