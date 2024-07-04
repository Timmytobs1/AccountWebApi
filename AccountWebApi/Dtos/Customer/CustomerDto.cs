using System.ComponentModel.DataAnnotations;

namespace AccountWebApi.Dtos.Customer
{
    public class CustomerDto
    {

        public Guid id { get; set; }

        public string FirstName { get; set; } = string.Empty;
 
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; }
        public int Tier { get; set; } = 1;
        public DateTime CrreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
