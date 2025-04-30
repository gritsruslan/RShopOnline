using AutoMapper;
using RShopAPI_Test.DTOs;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Mapping;

public class ChangePasswordRequestProfile : Profile
{
    public ChangePasswordRequestProfile()
    {
        CreateMap<ChangePasswordRequest, ChangePasswordCommand>();
    }
}