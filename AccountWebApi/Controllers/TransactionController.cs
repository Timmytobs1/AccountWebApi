﻿using AccountWebApi.Dtos.Transaction;
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
        public async Task <IActionResult> Deposit([FromBody] DepositDto depositDto)
        {
            try
            {
                var flutterwave = new FlutterwaveService();
                var transaction = _repository.Deposit(depositDto);
                var response = await flutterwave.MakePayment(depositDto.Amount, "test@test.com");
                var responseBody = JsonDocument.Parse(response);
                JsonElement root = responseBody.RootElement;
                string status = root.GetProperty("status").GetString();
               
                if (transaction == null || status != "success")
                {
                    return BadRequest(new { Status = "Failed", Message = "Something Went Wrong", TransactionStatus = "Failed" });
                }           
                return Ok(new { Status = "Success", TransactionStatus = transaction.TransactionStatus, Data = transaction });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Status = "Failed", Message = ex.Message, TransactionStatus = "Failed" });
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
                var transaction = _repository.Transfer(transferDto);
                var response = await flutterwave.MakeTransfer(transferDto.Amount, transferDto.SourceAccountNo, transferDto.DestinationAccountNo, "test@test.com");
                var responseBody = JsonDocument.Parse(response);
                JsonElement root = responseBody.RootElement;
                string status = root.GetProperty("status").GetString();

                if (status == "success")
                {
                    if (transaction == null || status != "success")
                    {
                        return BadRequest(new { Status = "Failed", Message = "Something Went Wrong", TransactionStatus = "Failed" });
                    }
                    return Ok(new { Status = "Success", TransactionStatus = transaction.TransactionStatus, Data = transaction });
                }
                else
                {
                    return BadRequest(new { Status = "Failed", Message = "Something Went Wrong", TransactionStatus = "Failed" });

                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Status = "Failed", Message = ex.Message });
            }
        }


        [HttpPost("withdraw")]
        public async Task <IActionResult> Withdraw([FromBody] WithdrawalDto withdrawDto)
        {
            try
            {
                var flutterwave = new FlutterwaveService();
                var transaction = _repository.Withdraw(withdrawDto);
                var response = await flutterwave.MakePayment(withdrawDto.Amount, "test@test.com");
                var responseBody = JsonDocument.Parse(response);
                JsonElement root = responseBody.RootElement;
                string status = root.GetProperty("status").GetString();
             
                if (transaction == null || status != "success")
                {
                    return BadRequest(new { Status = "Failed", Message = "Account does not exist", TransactionStatus = "Failed" });
                }
                return Ok(new { Status = "Success", Message = "Transaction Successfully", TransactionStatus = transaction.TransactionStatus, Data = transaction });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Status = "Failed", Message = ex.Message, TransactionStatus = "Failed" });
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
