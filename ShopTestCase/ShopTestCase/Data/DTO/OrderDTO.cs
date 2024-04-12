namespace ShopTestCase.Data.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; private set; }
        public string CustomerFullName { get; set; }
        public string CustomerPhone { get; set; }
        public List<ProductResponse> Products { get; set; }
    }
}
