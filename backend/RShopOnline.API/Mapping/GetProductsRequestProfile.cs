using AutoMapper;
using RShopAPI_Test.DTOs;

namespace RShopAPI_Test.Mapping;

public class GetProductsRequestProfile : Profile
{
    public GetProductsRequestProfile()
    {
        CreateMap<GetProductsRequest, GetProductsRequest>().ReverseMap();
    }
}