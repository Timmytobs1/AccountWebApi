using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountWebApi.Model
{
    [Index("CustomerId", IsUnique = true)]
    [Index("AccountNo", IsUnique = true)]

    public class Account
    {
        public Guid Id { get; set; }
        public string AccountNo { get; set; }
        public decimal AccountBalance { get; set; } = 0;
        public Guid CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
