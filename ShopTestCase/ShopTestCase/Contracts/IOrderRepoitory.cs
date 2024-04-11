using ShopTestCase.Entities;

namespace ShopTestCase.Contracts
{
    public interface IOrderRepoitory
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetOrder(int id);
        Task CreateOrder(Order order);
    }
}
