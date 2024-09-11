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
                var failed = new Model.Transaction
                {
                    AccountNo = depositDto.AccountNo,
                    Amount = depositDto.Amount,
                    TransactionType = TransactionType.DEPOSIT,
                   TransactionStatus = "Failed"
                };
                _context.Transactions.Add(failed);
                _context.SaveChanges();
                throw new Exception("Account Does Not Exist");
            }

            var customer = _context.Customers.FirstOrDefault(y => y.id == account.CustomerId);

            if (depositDto.Amount < 0)
            {
                var failed = new Model.Transaction
                {
                    AccountNo = account.AccountNo,
                    AccountId = account.Id,
                    Amount = depositDto.Amount,
                    CustomerId = account.CustomerId,
                    TransactionType = TransactionType.DEPOSIT,
                   TransactionStatus = "Failed"
                };
                _context.Transactions.Add(failed);
                _context.SaveChanges();
                throw new Exception("Invalid amount");
            }

            if ((customer.Tier == 1 && account.AccountBalance + depositDto.Amount > 50000) ||
                (customer.Tier == 2 && account.AccountBalance + depositDto.Amount > 100000))
            {
                var failed = new Model.Transaction
                {
                    AccountNo = account.AccountNo,
                    AccountId = account.Id,
                    Amount = depositDto.Amount,
                    CustomerId = account.CustomerId,
                    TransactionType = TransactionType.DEPOSIT,
                  TransactionStatus = "Failed"
                };
                _context.Transactions.Add(failed);
                _context.SaveChanges();
                throw new Exception("Tier Balance Exceeded");
            }

            account.AccountBalance += depositDto.Amount;

            var transaction = new Model.Transaction
            {
                AccountNo = account.AccountNo,
                AccountId = account.Id,
                Amount = depositDto.Amount,
                CustomerId = account.CustomerId,
                TransactionType = TransactionType.DEPOSIT,
                TransactionStatus = "Successful"
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            var transactionDto = new TransactionDto
            {
                id = transaction.id,
                AccountId = account.Id,
                Amount = depositDto.Amount,
                CustomerId = account.CustomerId,
                TransactionType = "Deposit",
                TransactionStatus = "Successful"
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
            var sourceAccount = _context.Accounts.FirstOrDefault(x => x.AccountNo == transferDto.SourceAccountNo);
            var destinationAccount = _context.Accounts.FirstOrDefault(x => x.AccountNo == transferDto.DestinationAccountNo);

            if (sourceAccount == null || destinationAccount == null)
            {
                var failedTransaction = new Model.Transaction
                {
                    AccountNo = transferDto.SourceAccountNo,
                    Amount = transferDto.Amount,
                    TransactionType = TransactionType.TRANSFER,
                   TransactionStatus = "Failed"
                };
                _context.Transactions.Add(failedTransaction);
                _context.SaveChanges();
                throw new Exception("One or both accounts do not exist.");
            }

            if (transferDto.Amount <= 0 || sourceAccount.AccountBalance < transferDto.Amount)
            {
                var failedTransaction = new Model.Transaction
                {
                    AccountNo = transferDto.SourceAccountNo,
                    Amount = transferDto.Amount,
                    TransactionType = TransactionType.TRANSFER,
                   TransactionStatus = "Failed"
                };
                _context.Transactions.Add(failedTransaction);
                _context.SaveChanges();
                throw new Exception(transferDto.Amount <= 0 ? "Invalid transfer amount." : "Insufficient funds in source account.");
            }

            sourceAccount.AccountBalance -= transferDto.Amount;
            destinationAccount.AccountBalance += transferDto.Amount;

            var sourceTransaction = new Model.Transaction
            {
                AccountNo = sourceAccount.AccountNo,
                AccountId = sourceAccount.Id,
                Amount = -transferDto.Amount,
                CustomerId = sourceAccount.CustomerId,
                TransactionType = TransactionType.TRANSFER,
               TransactionStatus = "Successful"
            };

            var destinationTransaction = new Model.Transaction
            {
                AccountNo = destinationAccount.AccountNo,
                AccountId = destinationAccount.Id,
                Amount = transferDto.Amount,
                CustomerId = destinationAccount.CustomerId,
                TransactionType = TransactionType.TRANSFER,
                TransactionStatus = "Successful"
            };

            _context.Transactions.Add(sourceTransaction);
            _context.Transactions.Add(destinationTransaction);
            _context.SaveChanges();

            return new TransactionDto
            {
                id = sourceTransaction.id,
                AccountId = sourceAccount.Id,
                Amount = transferDto.Amount,
                CustomerId = sourceAccount.CustomerId,
                TransactionType = "Transfer",
                TransactionStatus = "Successful"
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
                var failed = new Model.Transaction
                {
                    AccountId = account.Id,
                    AccountNo = withdrawDto.AccountNo,
                    Amount = withdrawDto.Amount,
                    CustomerId = account.CustomerId,
                    TransactionType = TransactionType.WITHDRAW,
                    TransactionStatus = "Failed"
                };          
                _context.Transactions.Add(failed);
                _context.SaveChanges();
                throw new Exception("Insufficient Balance");
            }

            if (withdrawDto.Amount < 0)
            {
                var failed = new Model.Transaction
                {
                    AccountId = account.Id,
                    AccountNo = withdrawDto.AccountNo,
                    Amount = withdrawDto.Amount,
                    CustomerId = account.CustomerId,
                    TransactionType = TransactionType.WITHDRAW,
                    TransactionStatus = "Failed"
                };

             
                _context.Transactions.Add(failed);
                _context.SaveChanges();
                throw new Exception("Invalid amount");
            }

            var customer = _context.Customers.FirstOrDefault(y => y.id == account.CustomerId);

            var transaction = new Model.Transaction
            {
                AccountId = account.Id,
                AccountNo = withdrawDto.AccountNo,
                Amount = withdrawDto.Amount,
                CustomerId = account.CustomerId,
                TransactionType = TransactionType.WITHDRAW,
                TransactionStatus = "Successful"

            };

            account.AccountBalance -= withdrawDto.Amount;
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            var transactionDto = new TransactionDto
            {
                id = transaction.id,
                AccountId = account.Id,
                Amount = withdrawDto.Amount,
                CustomerId = account.CustomerId,
                TransactionType = "Withdraw",
                TransactionStatus = "Successful"
            };
            return transactionDto;
        }
    }
}
