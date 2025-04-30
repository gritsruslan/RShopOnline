using AutoMapper;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Entities;

namespace RShopAPI_Test.Storage.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, User>();
    }
}