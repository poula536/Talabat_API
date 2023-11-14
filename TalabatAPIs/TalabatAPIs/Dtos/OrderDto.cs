﻿using Talabat.Core.Entities.Order_Aggregate;

namespace TalabatAPIs.Dtos
{
    public class OrderDto
    {
        public  string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto ShippingAddress { get; set; }
    }
}
