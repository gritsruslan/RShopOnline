using AutoMapper;
using RShopAPI_Test.DTOs;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Mapping;

public class LoginRequestProfile : Profile
{
    public LoginRequestProfile()
    {
        CreateMap<LoginRequest, LoginCommand>();
    }
}