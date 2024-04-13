using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShopTestCase.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; private set; }
        public string CustomerFullName { get; set; }
        public string CustomerPhone { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
        public Order()
        {
            CreatedOn = DateTime.Now;
        }
    }
}
