using AutoMapper;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Entities;

namespace RShopAPI_Test.Storage.Mapping;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderEntity, Order>();
    }
}