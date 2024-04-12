using ShopTestCase.Data.Entities;

namespace ShopTestCase.Contracts
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetOrder(int id);
        Task CreateOrder(Order order);
    }
}
