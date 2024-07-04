using AccountWebApi.Data;
using AccountWebApi.Dtos.Account;
using AccountWebApi.Interface;
using AccountWebApi.Mappers;
using AccountWebApi.Model;

namespace AccountWebApi.Respository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public AccountDto CreateAccount(Guid customerId)
        {
            var account = new Account
            {
                CustomerId = customerId,
                AccountNo = GenerateAccountNumber()
            };

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return account.ToAccountDtoFromModel();
        }

        public string GenerateAccountNumber()
        {
            Random random = new Random();
            string digits = "2345678910";
            string accountNo = "";
            int length = 10;
            for (int i = 0; i < length; i++)
            {
                accountNo += digits[random.Next(digits.Length)];
            }
            return accountNo;
        }

        public AccountDto? GetAccountById(Guid id)
        {
            var accounts = _context.Accounts.FirstOrDefault(x => x.Id == id);
            if (accounts == null)
            {
                return null;
            }
            return accounts.ToAccountDtoFromModel();
        }

        public List<AccountDto> GetAllAccounts()
        {
            var accounts = _context.Accounts.Select(x => x.ToAccountDtoFromModel()).ToList();
            return accounts;
        }
    }
}
