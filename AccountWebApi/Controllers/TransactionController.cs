using AccountWebApi.Dtos.Transaction;
using AccountWebApi.Interface;
using AccountWebApi.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace AccountWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _repository;
        public TransactionController(ITransactionRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("Deposit")]
        public IActionResult Deposit([FromBody] DepositDto depositDto)
        {
            try
            {
                var flutterwave = new FlutterwaveService();
            
                var transaction = _repository.Deposit(depositDto);
                if (transaction == null)
                {
                    return BadRequest(new { Status = "Failed", Message = "Something Went Wrong" });
                }
                var response = flutterwave.MakePayment(depositDto.Amount, "test@test.com");

              
                return Ok(new { Status = "Success", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Status = "Failed", Message = ex.Message });
            }
        }
        /*  [HttpPost]
          [Route("Transfer")]
          public IActionResult Transfer([FromBody] TransferDto transferDto)
          {
              try
              {
                  var transaction = _repository.Transfer(transferDto);
                  if (transaction == null)
                  {
                      return BadRequest(new { Status = "Failed", Message = "Something Went Wrong" });
                  }

                  return Ok(new { Status = "Success", Data = transaction });
              }
              catch (Exception ex)
              {
                  return BadRequest(new { Status = "Failed", Message = ex.Message });
              }
          }
  */
        [HttpPost]
        [Route("Transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferDto transferDto)
        {
            try
            {
                var flutterwave = new FlutterwaveService();
                var response = await flutterwave.MakeTransfer(transferDto.Amount, transferDto.SourceAccountNo, transferDto.DestinationAccountNo, "test@test.com");
                var responseBody = JsonDocument.Parse(response);
                JsonElement root = responseBody.RootElement;
                string status = root.GetProperty("status").GetString();

                if (status == "success")
                {
                    var transaction = _repository.Transfer(transferDto);
                    if (transaction == null)
                    {
                        return BadRequest(new { Status = "Failed", Message = "Something Went Wrong" });
                    }
                    return Ok(new { Status = "Success", Data = transaction });


                }
                else
                {
                    return BadRequest(new { Status = "Failed", Message = "Something Went Wrong" });

                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Status = "Failed", Message = ex.Message });
            }
        }


        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] WithdrawalDto withdrawDto)
        {
            try
            {
                var transaction = _repository.Withdraw(withdrawDto);
                if (transaction == null)
                {
                    return BadRequest(new { Status = "Failed", Message = "Account does not exist" });
                }
                return Ok(new { Status = "Success", Message = "Transaction Successfully", Data = transaction });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Status = "Failed", Message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetTransaction()
        {
            var transaction = _repository.GetAllTransactions();
            return Ok(new { Status = "Success", Data = transaction });
        }
        [HttpGet("{customerId}")]
        public IActionResult GetTransaction([FromRoute] Guid customerId)
        {
            var transaction = _repository.GetTransactionById(customerId);
            if (transaction == null)
            {
                return BadRequest(new { Status = "Failed", Message = "Customer record does not exist" });
            }
            return Ok(new { Status = "Success", Data = transaction });
        }
    }
}
