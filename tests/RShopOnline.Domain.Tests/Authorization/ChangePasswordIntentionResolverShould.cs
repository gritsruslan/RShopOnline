using FluentAssertions;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Services.Authentication;
using RShopAPI_Test.Services.Authorization.Resolvers;

namespace RShopOnline.Domain.Tests.Authorization;

public class ChangePasswordIntentionResolverShould
{
    private readonly ChangePasswordIntentionResolver sut = new();
    
    [Theory]
    [InlineData(Role.Manager)]
    [InlineData(Role.Customer)]
    [InlineData(Role.Admin)]
    public void ReturnTrue_WhenUserIsNotCustomer(Role role)
    {
        var user = new RecognizedUser(Guid.Parse("AB156C01-24B6-41E1-92A9-CCD5F7015894"), role);
        sut.IsAllowed(user).Should().BeTrue();
    }
    
    [Fact]
    public void ReturnFalse_WhenUserIsGuest()
    {
        var user = RecognizedUser.Guest;
        sut.IsAllowed(user).Should().BeFalse();
    }
}