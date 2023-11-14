using AutoMapper;
using Microsoft.Extensions.Configuration;
using Talabat.Core.Entities.Order_Aggregate;
using TalabatAPIs.Dtos;

namespace TalabatAPIs.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration Configuration;
        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{Configuration["BaseApiUrl"]}{source.Product.PictureUrl}";
            return null;

        }
    }
}
