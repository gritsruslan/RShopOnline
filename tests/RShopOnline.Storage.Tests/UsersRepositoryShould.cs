using FluentAssertions;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Storage.Entities;
using RShopAPI_Test.Storage.Repositories;

namespace RShopOnline.Storage.Tests;

public class UsersRepositoryFixture() : StorageTestFixture
{
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        
        await using var dbContext = GetDbContext();
        await dbContext.Users.AddRangeAsync(new UserEntity()
        {
            Id = Guid.Parse("CBFE3AC8-A66E-47BD-8688-7C339F189F12"),
            Name = "name1",
            Email = "user1@gmail.com",
            PasswordHash = [1],
            Salt = [2],
            Role = Role.Admin,
        }, new UserEntity()
        {
            Id = Guid.Parse("1B118031-4566-4B0B-A171-40E9E4C8BE6B"),
            Name = "name2",
            Email = "user2@gmail.com",
            PasswordHash = [2],
            Salt = [1],
            Role = Role.Customer,
        });
        await dbContext.SaveChangesAsync();
    }
}


public class UsersRepositoryShould(UsersRepositoryFixture fixture) : IClassFixture<UsersRepositoryFixture>
{
    private readonly UsersRepository Sut = new(fixture.GetDbContext(), fixture.GetMapper());

    [Fact]
    public async Task CorrectAddUserAndGetUserByEmail()
    {
        var email = "newuser@gmail.com";
        await Sut.CreateUser("name", email, [1,2,3], [3, 2, 1], Role.Customer, CancellationToken.None);
        var userExists = await Sut.GetUserByEmail(email, CancellationToken.None);
        userExists.Should().NotBeNull();
    }

    [Fact]
    public async Task CorrectGetUserById()
    {
        var id = Guid.Parse("CBFE3AC8-A66E-47BD-8688-7C339F189F12");
        var user = await Sut.GetUserById(id, CancellationToken.None);
        user.Should().NotBeNull();
        user.Email.Should().Be("user1@gmail.com");
    }

    [Fact]
    public async Task CorrectUpdatePassword()
    {
        var id = Guid.Parse("1B118031-4566-4B0B-A171-40E9E4C8BE6B");
        byte[] newPasswordHash = [52, 52];
        await Sut.UpdatePassword(id, newPasswordHash, CancellationToken.None);
        
        var user = await Sut.GetUserById(id, CancellationToken.None);
        user.Should().NotBeNull();
        user.PasswordHash.Should().BeEquivalentTo(newPasswordHash);
    }
}