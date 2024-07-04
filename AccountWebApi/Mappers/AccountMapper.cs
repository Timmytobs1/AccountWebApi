using AccountWebApi.Dtos.Account;
using AccountWebApi.Dtos.Customer;
using AccountWebApi.Model;

namespace AccountWebApi.Mappers
{
    public static class AccountMapper
    {
        public static AccountDto ToAccountDtoFromModel(this Account account)
        {
            return new AccountDto
            {
                CustomerId = account.CustomerId,
                AccountNo = account.AccountNo,
                AccountBalance = account.AccountBalance,
                CreatedAt = account.CreatedAt,
            };
        }

    }
}
