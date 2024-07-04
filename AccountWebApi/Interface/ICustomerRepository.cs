using AccountWebApi.Dtos.Customer;

namespace AccountWebApi.Interface
{
    public interface ICustomerRepository
    {
        public List<CustomerDto> GetAllCustomer();

        public CustomerDto? GetCustomer(Guid id);

        public CustomerDto RegisterCustomer(RegisterCustomerDto registerdto);

        public CustomerDto? UpdateCustomer(UpdateCustomerDto customerDto);
        public CustomerDto? DeleteCustomer(Guid id);
        public bool UpgradeTier(Guid id);
        public bool CheckCustomer(Guid id);
    }
}
