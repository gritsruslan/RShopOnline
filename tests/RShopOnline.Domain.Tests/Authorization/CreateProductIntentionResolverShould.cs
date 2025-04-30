using FluentAssertions;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Services.Authentication;
using RShopAPI_Test.Services.Authorization.Resolvers;

namespace RShopOnline.Domain.Tests.Authorization;

public class CreateProductIntentionResolverShould
{
    private readonly CreateProductIntentionResolver sut = new();
    
    [Theory]
    [InlineData(Role.Manager)]
    [InlineData(Role.Admin)]
    public void ReturnTrue_WhenUserIsAdminOrManager(Role role)
    {
        var user = new RecognizedUser(Guid.Parse("B5B6DC09-D2A0-46D7-A87C-5DC19FEC36D8"), role);
        sut.IsAllowed(user).Should().BeTrue();
    }
    
    [Theory]
    [InlineData(Role.Guest)]
    [InlineData(Role.Customer)]
    public void ReturnFalse_WhenUserIsNotAdminOrManager(Role role)
    {
        var user = new RecognizedUser(Guid.Parse("B5B6DC09-D2A0-46D7-A87C-5DC19FEC36D8"), role);
        sut.IsAllowed(user).Should().BeFalse();
    }
}