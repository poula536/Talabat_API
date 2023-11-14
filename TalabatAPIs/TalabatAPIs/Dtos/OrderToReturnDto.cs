using System.Collections.Generic;
using System;
using Talabat.Core.Entities.Order_Aggregate;

namespace TalabatAPIs.Dtos
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        public Address ShippingAddress { get; set; }
        public string DeliveruMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } // Navegational Property [Many]
        public decimal SubTotal { get; set; }
        public string PaymentIntentId { get; set; }
        public decimal Total { get; set; }

    }
}
