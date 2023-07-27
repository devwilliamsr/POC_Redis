using POC_Redis.Models;

namespace POC_Redis.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public async Task<Customer> GetCustomer(Guid id)
        {
            //Simulando possivel lentidao na consulta dos dados
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

            //Mock
            var fakeFirst = await Task.FromResult(GenerateFakeCustomer().FirstOrDefault());
            fakeFirst.Id = id;
            return fakeFirst;
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            //Simulando possivel lentidao na consulta dos dados
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));

            return await Task.FromResult(GenerateFakeCustomer());
        }

        private IEnumerable<Customer> GenerateFakeCustomer()
        {
            return Enumerable.Range(1, 20).Select(index => new Customer
            {
                Id = Guid.NewGuid(),
                Name = $"Customer {index}"
            });
        }
    }
}
