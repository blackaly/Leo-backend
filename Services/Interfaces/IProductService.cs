﻿using Leo.Model.Domains;
using Leo.Model.DTOs;

namespace Leo.Services.Interfaces
{
    public interface IProductService
    {
        Task<IList<Product>> ReadProducts(int customerId);
        Task<IList<Product>> GetProducts();
        Task<Product> Add(Product dto);
        Task Update(Product dto);
        Task Remove(Product dto);
        Task<bool> isProductExists(int productId);
        Task<Product> GetProductBy(int productId);
    }
}
