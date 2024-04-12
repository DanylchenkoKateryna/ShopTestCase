﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ShopTestCase.Entities
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Amount { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        public decimal TotalPrice { get { return Amount* (Product?.Price ?? 0);} set { } }

    }
}

