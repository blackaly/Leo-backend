using Leo.Model;
using Leo.Model.Domains;
using Leo.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Leo.Services.Implementation
{
    public class CustomerService : ICustomerService
    {

        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> Add(Customer dto)
        {
            await _context.Customers.AddAsync(dto);
            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<IList<Customer>> GetAllCustomer()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerBy(int Id)
        {
            return await _context.Customers.FindAsync(Id);
        }

        public async Task<IList<object>> GetCustomerNames()
        {
            var customers =  _context.Customers.Select(x => new
            {
                x.Name,
                x.CustomerId
            });

            return await customers.Cast<object>().ToListAsync();
        }

        public bool Remove(Customer dto)
        {
            _context.Customers.Remove(dto);
            bool deleted = true;
            var entry = _context.Entry(dto);
            if (entry.State == EntityState.Deleted) return deleted;
            return !deleted;

        }

        public Customer Update(Customer dto)
        {
            _context.Customers.Update(dto);
            var entry = _context.Customers.Entry(dto);
            if (entry.State == EntityState.Modified) return dto;
            return null;
        }
    }
}
