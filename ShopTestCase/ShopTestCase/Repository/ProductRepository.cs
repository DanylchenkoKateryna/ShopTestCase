using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShopTestCase.Contracts;
using ShopTestCase.Data;
using ShopTestCase.Data.Entities;

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
            string sqlQuery = "SELECT * FROM Products WHERE Id = @id";

            SqlParameter parameter = new SqlParameter("@id", id);

            return await _context.Products.FromSqlRaw(sqlQuery, parameter).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByCode(string code)
        {
            string sqlQuery = "SELECT * FROM Products WHERE Code = @code";

            SqlParameter parameter = new SqlParameter("@code", code);

            return await _context.Products.FromSqlRaw(sqlQuery, parameter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.FromSqlRaw($"Select * from Products").ToListAsync();
        }

        public async Task UpdateProduct( Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
