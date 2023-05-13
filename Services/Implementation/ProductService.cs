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

        public async Task Remove(Product dto)
        {
            _context.Products.Remove(dto);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Product dto)
        {
             _context.Products.Update(dto);
             await _context.SaveChangesAsync();
        }
    }
}
