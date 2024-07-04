using AccountWebApi.Dtos.Customer;
using AccountWebApi.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AccountWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _repository;

        public CustomerController(ICustomerRepository repository)
        {
            _repository = repository;


        }

        [HttpPost]
        public IActionResult RegisterCustomer([FromBody] RegisterCustomerDto customerDto)
        {
            try
            {
                var customer = _repository.RegisterCustomer(customerDto);
                return Ok(new { Status = "Success", Message = "Customer Created Successfully", Data = customer });
            }
            catch (Exception)
            {
                return BadRequest(new { Status = "Failed", Message = "Duplicate Entry" });
            }
        }

        [HttpGet]
        public IActionResult GetAllCustomer()
        {
            var customer = _repository.GetAllCustomer();
            return Ok(new { Status = "Success", Message = customer });
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomersById([FromRoute] Guid id)
        {
            var customer = _repository.GetCustomer(id);
            if (customer == null)
            {
                return BadRequest(new { Status = "Failed", Message = "Customer does not exist" });
            }
            return Ok(new { Status = "Success", Message = customer });
        }



        [HttpPatch("upgrade/{customerId}")]
        public IActionResult UpgradeTier([FromRoute] Guid customerId)
        {
            if (_repository.UpgradeTier(customerId))
            {
                return Ok(new { Status = "Success", Message = "Upgrade successfully" });
            }
            return BadRequest(new { Status = "Failed", Message = "Could not upgrade tier" });

        }
    }

}
