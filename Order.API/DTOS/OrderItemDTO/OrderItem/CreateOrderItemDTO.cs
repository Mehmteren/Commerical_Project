﻿namespace Order.API.DTOS.OrderItemDTO.OrderItem
{
    public class CreateOrderItemDTO
    {
        public int ProductId { get; set; }
        public int ShippingId { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
