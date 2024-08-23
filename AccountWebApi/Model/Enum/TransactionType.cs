using System.ComponentModel.DataAnnotations;

namespace AccountWebApi.Model.Enum
{
    public enum TransactionType
    {
        [Display(Name = "Withdraw")]
        WITHDRAW,
        [Display(Name = "Deposit")]
        DEPOSIT,
        [Display(Name = "Transfer")]
        TRANSFER,
    }
}
