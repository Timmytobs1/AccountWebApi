using AccountWebApi.Dtos.Account;

namespace AccountWebApi.Interface
{
    public interface IAccountRepository
    {
        public List<AccountDto> GetAllAccounts();
        public AccountDto? GetAccountById(Guid id);
        public AccountDto CreateAccount(Guid customerId);
        public string GenerateAccountNumber();


    }
}
