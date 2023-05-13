using Leo.Model.Domains;

namespace Leo.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerBy(int Id);
        Task<IList<Customer>> GetAllCustomer();
        Task<Customer> Add(Customer dto);
        Customer Update(Customer dto);
        bool Remove(Customer dto);
        Task<IList<object>> GetCustomerNames();
    }
}
