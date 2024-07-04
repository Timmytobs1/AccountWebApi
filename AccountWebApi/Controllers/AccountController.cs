using AccountWebApi.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AccountWebApi.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _repository;
        public AccountController(IAccountRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult CreateAccount([FromBody] Guid customerId)
        {
            var account = _repository.CreateAccount(customerId);
            return Ok(account);
        }

        [HttpGet]
        public IActionResult GetAllAccounts()
        {
            var account = _repository.GetAllAccounts();
            return Ok(new { Status = "Success", Message = account });
        }
    }
}
