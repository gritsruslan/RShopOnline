using AutoMapper;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.DTOs;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Mapping;

public class CreateOrderRequestProfile : Profile
{
    public CreateOrderRequestProfile()
    {
        CreateMap<OrderItemServiceDto, OrderItemDto>();
        CreateMap<CreateOrderRequest, CreateOrderCommand>();
    }
}