using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AccountWebApi.Model
{
    [Index("Email", IsUnique =true)]
    [Index("Phone", IsUnique =true)]
    public class Customer
    {
        public Guid id { get; set; }
        [StringLength(30)]
        public string FirstName { get; set; } = string.Empty;
        [StringLength(30)]
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone {  get; set; } 
        public int Tier { get; set; } = 1;
        public DateTime CrreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set;} = DateTime.Now;
    }
}
