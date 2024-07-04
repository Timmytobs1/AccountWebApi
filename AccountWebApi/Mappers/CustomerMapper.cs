using AccountWebApi.Dtos.Customer;
using AccountWebApi.Model;

namespace AccountWebApi.Mappers
{
    public static class CustomerMapper
    {
        public static Customer ToCustomerModel(this RegisterCustomerDto registerCustomerDto)
        {
            return new Customer
            {
              FirstName = registerCustomerDto.FirstName,
              LastName = registerCustomerDto.LastName,
              Email = registerCustomerDto.Email,
              Phone = registerCustomerDto.Phone,
              Address = registerCustomerDto.Address,    
            };
        }

        public static CustomerDto ToCustomerDto(this Customer customer)
        {
            return new CustomerDto
            {
                id = customer.id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                Tier = customer.Tier,
                CrreatedAt = customer.CrreatedAt,
                UpdatedAt = customer.UpdatedAt,
            };
        }
    }
}
