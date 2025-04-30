using AutoMapper;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Entities;

namespace RShopAPI_Test.Storage.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductEntity, Product>()
            .ForSourceMember(src => src.CategoryId, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.Category, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.OrderItems, opt => opt.DoNotValidate());
    }
}