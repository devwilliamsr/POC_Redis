using POC_Redis.Models;
using POC_Redis.Repositories;
using System.Net.Http.Json;

namespace POC_Redis.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICacheRepository cacheRepository;
        private readonly ICustomerRepository customerRepository;
        private const string CACHE_COLLECTION_KEY = "_AllCustomers";

        public CustomerService(
            ICacheRepository cacheRepository,
            ICustomerRepository customerRepository)
        {
            this.cacheRepository = cacheRepository 
                ?? throw new ArgumentNullException(nameof(cacheRepository));
            this.customerRepository = customerRepository 
                ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<Customer> GetCustomer(Guid id)
        {
            var customer = await cacheRepository.GetValue<Customer>(id);

            if (customer is null) 
            { 
                customer = await customerRepository.GetCustomer(id);
                await cacheRepository.SetValue(id, customer);
            }

            return customer;
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            var customers = await cacheRepository.GetCollection<Customer>(CACHE_COLLECTION_KEY); 

            if(customers is null || !customers.Any())
            {
                customers = await customerRepository.GetCustomers();
                await cacheRepository.SetCollection(CACHE_COLLECTION_KEY, customers);
            }

            return customers;
        }

        public async Task<IEnumerable<Customer>> GetExternalCustomers()
        {
            var cacheKey = "_ExternalCustomers";
            var customers = await cacheRepository.GetCollection<Customer>(cacheKey);

            if(customers is null || !customers.Any())
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync("http://localhost:9091/api/customers");

                    if (response.IsSuccessStatusCode)
                    {
                        var custumers = await response.Content.ReadFromJsonAsync<List<Customer>>();

                        await cacheRepository.SetCollection(cacheKey, custumers);
                    }
                }
            }

            return customers;
        }
    }
}
