﻿namespace AccountWebApi.Dtos.Customer
{
    public class RegisterCustomerDto
    {
    
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; }
      

    }
}
