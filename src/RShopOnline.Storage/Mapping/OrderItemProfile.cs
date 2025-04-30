using AutoMapper;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Entities;

namespace RShopAPI_Test.Storage.Mapping;

public class OrderItemProfile : Profile
{
    public OrderItemProfile()
    {
        CreateMap<OrderItemEntity, OrderItem>();
    }
}