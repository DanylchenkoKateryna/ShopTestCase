using Microsoft.EntityFrameworkCore;
using ShopTestCase.Contracts;
using ShopTestCase.Data;
using ShopTestCase.Entities;

namespace ShopTestCase.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopContext _context;
        public ProductRepository(ShopContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
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

        public async Task UpdateProduct( Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
