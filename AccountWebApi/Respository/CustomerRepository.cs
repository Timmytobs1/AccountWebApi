using AccountWebApi.Data;
using AccountWebApi.Dtos.Customer;
using AccountWebApi.Interface;
using AccountWebApi.Mappers;
using Microsoft.EntityFrameworkCore;

namespace AccountWebApi.Respository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CheckCustomer(Guid id)
        {
           var check = _context.Customers.Any(x => x.id == id);
            return check;   
        }

        public CustomerDto? DeleteCustomer(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<CustomerDto> GetAllCustomer()
        {
            var customers = _context.Customers.Select(b => b.ToCustomerDto()).ToList();

            return customers;
        }

        public CustomerDto? GetCustomer(Guid id)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.id == id);
            if (customer == null)
            {
                return null;
            }
            return customer.ToCustomerDto();
        }

        public CustomerDto RegisterCustomer(RegisterCustomerDto registerdto)
        {
            var customer = registerdto.ToCustomerModel();
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return customer.ToCustomerDto();
        }

        public CustomerDto? UpdateCustomer(UpdateCustomerDto customerDto)
        {
            throw new NotImplementedException();
        }

        public bool UpgradeTier(Guid id)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.id == id);
            if (customer == null)
            {
                return false;
            }
            if (customer.Tier > 2)
            {
                return false;
            }
            customer.Tier += 1;
            _context.SaveChanges();
            return true;
        }
    }
}
