using Leo.Model.Domains;
using Leo.Model.DTOs;
using Leo.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private IWebHostEnvironment _webHostEnvironment;

        public CustomerController(ICustomerService customerService, IWebHostEnvironment webHostEnvironment)
        {
            _customerService = customerService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("Names")]
        public async Task<IActionResult> GetAllCustomersNames()
        {
            return Ok(await _customerService.GetCustomerNames());
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllCustomers()
        {

            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads\\Customers\\");
            var obj = await _customerService.GetAllCustomer();

            List<CustomerTransferDTO> result = new List<CustomerTransferDTO>();

            foreach(var i in obj)
            {
                string realPath = string.Concat(path, i.ImageUrl);
                if (System.IO.File.Exists(realPath))
                {
                    MemoryStream m = new MemoryStream();
                    using FileStream f = new FileStream(realPath, FileMode.Open);
                    f.CopyTo(m);
                    result.Add(new CustomerTransferDTO {Name = i.Name, customerId = i.CustomerId, customerLogo = Convert.ToBase64String(m.ToArray()) });
                    m.Flush();

                }
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerBy(int customerId)
        {
            if (await _customerService.GetCustomerBy(customerId) is null) return NoContent();
            var customer = await _customerService.GetCustomerBy(customerId);

            CustomersDTO dto = new CustomersDTO()
            {
                Name = customer.Name,
            };

            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads\\Customers", customer.ImageUrl);

            if(System.IO.File.Exists(path))
            {
                MemoryStream m = new MemoryStream();
                FileStream f = new FileStream(path, FileMode.Open);
                Console.WriteLine(f.Length);
                f.CopyTo(m);
                customer.ImageUrl = Convert.ToBase64String(m.ToArray());
                m.Flush();

            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromForm]CustomersDTO dto)
        {
            if(!ModelState.IsValid) { return BadRequest(ModelState); }

            string fakeName = Path.GetRandomFileName();

            Customer customer = new Customer()
            {
                Name = dto.Name,
                ImageUrl= fakeName,
            };

            try
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads\\Customers", fakeName);
                using FileStream f = new FileStream(path, FileMode.Create);
                dto.ImageUrl.CopyTo(f);
                await _customerService.Add(customer);
                return Ok(customer);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }   
        }

    }
}
