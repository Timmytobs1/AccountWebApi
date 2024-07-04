using System.ComponentModel.DataAnnotations.Schema;

namespace AccountWebApi.Dtos.Account
{
    public class AccountDto
    {
        public Guid CustomerId { get; set; }
        public string AccountNo { get; set; }
        public decimal AccountBalance { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
