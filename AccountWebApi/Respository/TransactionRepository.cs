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

      

        public TransactionDto Transfer(TransferDto transferDto)
        {
            // Retrieve the source and destination accounts
            var sourceAccount = _context.Accounts.FirstOrDefault(x => x.AccountNo == transferDto.SourceAccountNo);
            var destinationAccount = _context.Accounts.FirstOrDefault(x => x.AccountNo == transferDto.DestinationAccountNo);

            if (sourceAccount == null || destinationAccount == null)
            {
                throw new Exception("One or both accounts do not exist.");
            }

            if (transferDto.Amount <= 0)
            {
                throw new Exception("Invalid transfer amount.");
            }

            if (sourceAccount.AccountBalance < transferDto.Amount)
            {
                throw new Exception("Insufficient funds in source account.");
            }

            var sourceCustomer = _context.Customers.FirstOrDefault(c => c.id == sourceAccount.CustomerId);
            var destinationCustomer = _context.Customers.FirstOrDefault(c => c.id == destinationAccount.CustomerId);

            // Check customer tiers and account balance limits
            if (sourceCustomer.Tier == 1)
            {
                if (sourceAccount.AccountBalance - transferDto.Amount < 0)
                {
                    throw new Exception("Tier Balance Exceeded.");
                }
            }
            else if (sourceCustomer.Tier == 2)
            {
                // Assuming no specific checks for tier 2
            }
            else if (sourceCustomer.Tier == 3)
            {
                // Assuming no specific checks for tier 3
            }

            // Update the balances
            sourceAccount.AccountBalance -= transferDto.Amount;
            destinationAccount.AccountBalance += transferDto.Amount;

            // Create transactions for both accounts
            var sourceTransaction = new Model.Transaction
            {
                AccountNo = sourceAccount.AccountNo,
                AccountId = sourceAccount.Id,
                Amount = -transferDto.Amount,
                CustomerId = sourceAccount.CustomerId,
                TransactionType = TransactionType.TRANSFER
            };

            var destinationTransaction = new Model.Transaction
            {
                AccountNo = destinationAccount.AccountNo,
                AccountId = destinationAccount.Id,
                Amount = transferDto.Amount,
                CustomerId = destinationAccount.CustomerId,
                TransactionType = TransactionType.TRANSFER
            };

            // Add transactions to the context
            _context.Transactions.Add(sourceTransaction);
            _context.Transactions.Add(destinationTransaction);

            _context.SaveChanges();

            // Return the transfer details as a DTO
            return new TransactionDto
            {
                id = sourceTransaction.id,
                AccountId = sourceAccount.Id,
                Amount = transferDto.Amount,
                CustomerId = sourceAccount.CustomerId,
                TransactionType = "Transfer",
            };
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
