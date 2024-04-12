namespace ShopTestCase.Data.DTO
{
    public class OrderForCreationResponse
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; private set; }
        public string CustomerFullName { get; set; }
        public string CustomerPhone { get; set; }
        public List<OrderProductDTO> Products { get; set; }
    }
}
