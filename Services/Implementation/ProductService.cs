using Leo.Model;
using Leo.Model.Domains;
using Leo.Model.DTOs;
using Leo.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Leo.Services.Implementation
{
    public class ProductService : IProductService
    {

        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context; 
        }

        public async Task<Product> Add(Product dto)
        {
            await _context.Products.AddAsync(dto);
            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<Product> GetProductBy(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<IList<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<bool> isProductExists(int productId)
        {
            return await _context.Products.AnyAsync(x => x.ProductId == productId);
        }

        public async Task<IList<Product>> ReadProducts(int customerId)
        {
            var obj = await _context.Products.Where(x => x.CustomerId == customerId).ToListAsync();
            return obj;
        }

        public bool Remove(Product dto)
        {
            _context.Products.Remove(dto);

            bool deleted = true;
            var entry = _context.Products.Entry(dto);
            if (entry.State == EntityState.Deleted) return deleted;
            return !deleted;
        }

        public  Product Update(Product dto)
        {
             _context.Products.Update(dto);
            var entry = _context.Products.Entry(dto);
            if (entry.State == EntityState.Modified) return dto;
            return null;
        }
    }
}
