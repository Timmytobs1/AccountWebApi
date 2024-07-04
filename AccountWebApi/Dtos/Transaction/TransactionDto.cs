using AccountWebApi.Model.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountWebApi.Dtos.Transaction
{
    public class TransactionDto
    {
        public Guid id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public Guid AccountId { get; set; }
      
    }
}
