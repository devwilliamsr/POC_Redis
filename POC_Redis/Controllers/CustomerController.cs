using Microsoft.AspNetCore.Mvc;
using POC_Redis.Services;
using System.Diagnostics;

namespace POC_Redis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomerController(
            ICustomerService customerService)
        {
            this.customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var customers = await customerService.GetCustomers();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            var result = new
            {
                duration = new { miliseconds = ts.Milliseconds, seconds = ts.Seconds },
                customers = customers,
            };

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await customerService.GetCustomer(id));
        }

        [HttpGet("externals")]
        public async Task<IActionResult> GetExternalCustomers()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var customers = await customerService.GetExternalCustomers();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            var result = new
            {
                duration = new { miliseconds = ts.Milliseconds, seconds = ts.Seconds },
                customers = customers,
            };

            return Ok(result);
        }
    }
}
