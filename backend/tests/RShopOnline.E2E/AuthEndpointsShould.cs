using System.Net.Http.Json;
using FluentAssertions;
using RShopAPI_Test.DTOs;
using RShopAPI_Test.Services.Commands;
using Xunit.Abstractions;

namespace RShopOnline.E2E;

public class AuthEndpointsShould(RShopApiApplicationFactory factory, ITestOutputHelper testOutputHelper) : IClassFixture<RShopApiApplicationFactory>
{
    public const string Uri = "http://localhost";
    
    [Fact]
    public async Task SuccessfullyregisterUser()
    {
        using var client = factory.CreateClient();
        using var response = await client.PostAsync("/api/auth/register",
            JsonContent.Create(new RegistrationRequest("Name", "asdfgasdg123@email.com", "StrongPass123")));
        
        response.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();
    }


    [Fact]
    public async Task NotRegisterUser_WhenUserWithThisEmailAlreadyExists()
    {
        using var client = factory.CreateClient();
        var email = "laskdjflaksdf@pornhub.com";
        
        using var response = await client.PostAsync("/api/auth/register",
            JsonContent.Create(new RegistrationRequest("Name", email ,"StrongPass123")));
        response.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();
        
        using var response2 = await client.PostAsync("/api/auth/register",
            JsonContent.Create(new RegistrationRequest("Name", email ,"StrongPass123")));
        response2.Invoking(r => r.EnsureSuccessStatusCode()).Should().Throw<HttpRequestException>();
    }

    [Fact]
    public async Task SuccessfullyLoginUser()
    {
        using var client = factory.CreateClient();
        var email = "laskdjflaksdf123123@pornhub.com";
        var password = "0000111popoPo";
        
        using var response = await client.PostAsync("/api/auth/register",
            JsonContent.Create(new RegistrationRequest("Name", email ,password)));
        response.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();

        using var loginResponse =
            await client.PostAsync("/api/auth/login", JsonContent.Create(new LoginRequest(email, password)));
        loginResponse.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();
    }

    [Fact]
    public async Task NotLoginUser_WhenNoUserWithThisEmail()
    {
        using var client = factory.CreateClient();
        var email = "dimon_zakon@pornhub.com";
        var password = "NNNNNNNNNNNNNN123n";

        using var response =
            await client.PostAsync("/api/auth/login", JsonContent.Create(new LoginRequest(email, password)));
        
        response.Invoking(r => r.EnsureSuccessStatusCode()).Should().Throw<HttpRequestException>();
    }

    [Fact]
    public async Task NotLoginUser_WhenPasswordIsIncorrect()
    {
        using var client = factory.CreateClient();
        var email = "myemail@gmaaaail.com";
        var password = "CorrectPassword52";
        var incorrectPassword = password + "1";
        
        using var response = await client.PostAsync("/api/auth/register", 
            JsonContent.Create(new RegistrationRequest("Name", email, password)));
        response.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();

        using var loginResponse = await client.PostAsync("/api/auth/login",
            JsonContent.Create(new LoginRequest(email, incorrectPassword)));
        loginResponse.Invoking(r => r.EnsureSuccessStatusCode()).Should().Throw<HttpRequestException>();

    }

    [Fact]
    public async Task DontChangePassword_WhenUserIsNotAuthenticated()
    {
        using var client = factory.CreateClient();
        using var response = await client.PostAsync("/api/auth/changepassword", JsonContent.Create(
                new ChangePasswordCommand("Passsssss123", "Passsssss1234")));
        response.IsSuccessStatusCode.Should().BeFalse();
    }

    [Fact]
    public async Task DontChangePassword_WhenPasswordIsIncorrect()
    {
        using var client = factory.CreateClient();
        
        var email = "myemail123@gmaaaail.com";
        var password = "myPass777888";
        var incorrectPassword = "myPass777888!";
        var newPassword = "1myPass777888";
        
        using var registerResponse = await client.PostAsync("/api/auth/register", 
            JsonContent.Create(new RegistrationRequest("Name", email, password)));
        registerResponse.IsSuccessStatusCode.Should().BeTrue();
        
        using var loginResponse =
            await client.PostAsync("/api/auth/login", JsonContent.Create(new LoginRequest(email, password)));
        loginResponse.IsSuccessStatusCode.Should().BeTrue();
        
        var changePasswordRequest = new HttpRequestMessage(HttpMethod.Post, "/api/auth/changepassword");
        changePasswordRequest.Content = JsonContent.Create(new ChangePasswordRequest(incorrectPassword, newPassword));

        using var changePassResponse = await client.SendAsync(changePasswordRequest);
        changePassResponse.IsSuccessStatusCode.Should().BeFalse();
    }

    [Fact]
    public async Task SuccessfullyChangePassword()
    {
        using var client = factory.CreateClient();
        
        var email = "myemail123z@gmaaaail.com";
        var password = "myPass777888";
        var newPassword = "1myPass777888";
        
        using var registerResponse = await client.PostAsync("/api/auth/register", 
            JsonContent.Create(new RegistrationRequest("Name", email, password)));
        registerResponse.IsSuccessStatusCode.Should().BeTrue();
        
        using var loginResponse =
            await client.PostAsync("/api/auth/login", JsonContent.Create(new LoginRequest(email, password)));
        loginResponse.IsSuccessStatusCode.Should().BeTrue();
        
        var changePasswordRequest = new HttpRequestMessage(HttpMethod.Post, "/api/auth/changepassword");
        changePasswordRequest.Content = JsonContent.Create(new ChangePasswordRequest(password, newPassword));

        using var changePassResponse = await client.SendAsync(changePasswordRequest);
        changePassResponse.IsSuccessStatusCode.Should().BeTrue();
    }
}