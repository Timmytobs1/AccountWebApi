using System.ComponentModel.DataAnnotations.Schema;
using AccountWebApi.Model.Enum;

namespace AccountWebApi.Model
{
    public class Transaction
    {

        public Guid id { get; set; } 
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public string TransactionStatus { get; set; }
        public Guid CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }
        [ForeignKey("AccountId")]
        public Guid AccountId { get; set; }
        public string AccountNo { get; set; }   
        public Account? Account { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
      
    }
}
