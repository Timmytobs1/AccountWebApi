using AccountWebApi.Data;
using AccountWebApi.Dtos.Transaction;
using AccountWebApi.Interface;
using AccountWebApi.Model;
using AccountWebApi.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountWebApi.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public TransactionDto Deposit(DepositDto depositDto)
        {
   
            var account = _context.Accounts.FirstOrDefault(x => x.AccountNo == depositDto.AccountNo);
            if (account == null)
            {
                throw new Exception("Account Does Not Exist");
            }

            var customer = _context.Customers.FirstOrDefault(y => y.id == account.CustomerId);

            var transaction = new Model.Transaction
            {
                AccountNo = account.AccountNo,
                AccountId = account.Id,
                Amount = depositDto.Amount,
                CustomerId = account.CustomerId,
                TransactionType = TransactionType.DEPOSIT
            };

            if (depositDto.Amount < 0)
            {
                throw new Exception("Invalid amount");
            }

            if (customer.Tier == 1)
            {
                if (account.AccountBalance + depositDto.Amount > 50000)
                {
                    throw new Exception("Tier Balance Exceeded");
                }
                else
                {
                    account.AccountBalance += depositDto.Amount;
                }
            }
            else if (customer.Tier == 2)
            {
                if (account.AccountBalance + depositDto.Amount > 100000)
                {
                    throw new Exception("Balance Exceeded");
                }
                else
                {
                    account.AccountBalance += depositDto.Amount;
                }
            }
            else if (customer.Tier == 3)
            {
                account.AccountBalance += depositDto.Amount;
            }

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            var transactionDto = new TransactionDto
            {
               
                AccountId = account.Id,
                Amount = depositDto.Amount,
                CustomerId = account.CustomerId,
                TransactionType = "Deposit",
            };

            return transactionDto;
        }

        public List<TransactionDto> GetAllTransactions()
        {
            throw new NotImplementedException();
        }

        public TransactionDto GetTransactionById(Guid id)
        {
            throw new NotImplementedException();
        }

        public TransactionDto Withdraw(WithdrawalDto withdrawDto)
        {

            var account = _context.Accounts.FirstOrDefault(x => x.AccountNo == withdrawDto.AccountNo);
            if (account == null)
            {
                throw new Exception("Account Does Not Exist");
            }

            if (account.AccountBalance < withdrawDto.Amount)
            {
                throw new Exception("Insufficient Balance");
            }

            if (withdrawDto.Amount < 0)
            {
                throw new Exception("Invalid amount");
            }

            var customer = _context.Customers.FirstOrDefault(y => y.id == account.CustomerId);

            var transaction = new Model.Transaction
            {
                AccountId = account.Id,
                AccountNo = withdrawDto.AccountNo,
                Amount = withdrawDto.Amount,
                CustomerId = account.CustomerId,
                TransactionType = TransactionType.WITHDRAW
            };

            account.AccountBalance -= withdrawDto.Amount;
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            var transactionDto = new TransactionDto
            {
                AccountId = account.Id,
                Amount = withdrawDto.Amount,
                CustomerId = account.CustomerId,
                TransactionType = "Withdraw",
            };

            return transactionDto;
        }
    }
}
