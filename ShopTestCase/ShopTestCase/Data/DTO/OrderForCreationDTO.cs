﻿

namespace ShopTestCase.Data.DTO
{
    public class OrderForCreationDTO
    {
        public string CustomerFullName { get; set; }
        public string CustomerPhone { get; set; }
        public List<OrderProductDTO> OrderProducts { get; set; }
    }
}
