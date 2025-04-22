using FluentAssertions;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Services.Authentication;
using RShopAPI_Test.Services.Authorization.Resolvers;

namespace RShopOnline.Tests.Authorization;

public class CancelOrderIntentionResolverShould
{
    private readonly CancelOrderIntentionResolver sut = new();

    [Fact]
    public void ReturnTrue_WhenUserIsCustomer()
    {
        var user = new RecognizedUser(Guid.Parse("83B334B6-8940-4395-9B33-26129F39E9E4"), Role.Customer);
        sut.IsAllowed(user).Should().BeTrue();
    }

    [Theory]
    [InlineData(Role.Manager)]
    [InlineData(Role.Admin)]
    [InlineData(Role.Guest)]
    public void ReturnFalse_WhenUserIsNotCustomer(Role role)
    {
        var user = new RecognizedUser(Guid.Parse("AB156C01-24B6-41E1-92A9-CCD5F7015894"), role);
        sut.IsAllowed(user).Should().BeFalse();
    }
}