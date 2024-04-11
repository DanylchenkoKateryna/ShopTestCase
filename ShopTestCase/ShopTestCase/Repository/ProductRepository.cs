using Microsoft.EntityFrameworkCore;
using ShopTestCase.Contracts;
using ShopTestCase.Data;
using ShopTestCase.Entities;

namespace ShopTestCase.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopContext _context;
        public ProductRepository(ShopContext _context)
        {
            _context = _context ?? throw new ArgumentNullException(nameof(_context));
        }
        public async Task CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetProduct(int id)
        {
            return await _context.Products.FromSql($"SELECT * FROM Products WHERE Id = {id}").FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByCode(string code)
        {
            return await _context.Products.FromSql($"SELECT * FROM Products WHERE Code = {code}").FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.FromSql($"Select * from Products").ToListAsync();
        }

        public async Task UpdateProduct(int id, Product product)
        {
            var existingProduct = await GetProduct(id);

            if (existingProduct != null)
            {
                existingProduct.Code = product.Code;
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;

                await _context.SaveChangesAsync();
            }
        }
    }
}
