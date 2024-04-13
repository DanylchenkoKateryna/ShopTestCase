using Microsoft.EntityFrameworkCore;
using ShopTestCase.Contracts;
using ShopTestCase.Data;
using ShopTestCase.Data.Entities;

namespace ShopTestCase.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ShopContext _context;
        public OrderRepository(ShopContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task CreateOrder(Order order)
        {
            _context.Orders.Add(order);

            // Calculate TotalPrice for each OrderProduct
            foreach (var item in order.OrderProducts)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                {
                    throw new Exception($"Product with ID {item.ProductId} not found.");
                }
                item.TotalPrice = item.Amount * product.Price;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrder(int id)
        {
            var order = await _context.Orders
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            var orders = await _context.Orders
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .ToListAsync();

            return orders;
        }
    }
}
