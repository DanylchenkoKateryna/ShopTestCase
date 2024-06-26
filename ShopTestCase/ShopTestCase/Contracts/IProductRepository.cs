﻿using ShopTestCase.Data.Entities;

namespace ShopTestCase.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(int id);
        Task<Product> GetProductByCode(string code);
        Task CreateProduct(Product product);
        Task UpdateProduct(Product product);
    }
}
