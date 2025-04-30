using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RShopAPI_Test.DTOs;
using RShopAPI_Test.Extensions;
using RShopAPI_Test.Services.Authentication;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Interfaces;

namespace RShopAPI_Test.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService service, IMapper mapper) : ControllerBase 
{
    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken ct)
    {
        var result = await service.Login(mapper.Map<LoginCommand>(request), ct);
        if (result.IsSuccess)
        {
            HttpContext.Response.Cookies.Append(CookieNames.AuthToken, result.Value);
        }
        return result.ToResponse();
    }


    [HttpPost("register")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Register(
        [FromBody] RegistrationRequest request, 
        CancellationToken ct)
    {
        var result = await service.Registration(mapper.Map<RegistrationCommand>(request), ct);
        return result.ToResponse();
    }


    [HttpPost("changepassword")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken ct)
    {
        var result = await service.ChangePassword(mapper.Map<ChangePasswordCommand>(request), ct);
        return result.ToResponse();
    }
}