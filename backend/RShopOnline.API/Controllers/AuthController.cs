using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RShopAPI_Test.DTOs;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Interfaces;

namespace RShopAPI_Test.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController(IAuthService service) : ControllerBase 
{
    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        IMapper mapper,
        CancellationToken ct)
    {
        var result = await service.Login(mapper.Map<LoginCommand>(request), ct);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
       
        HttpContext.Response.Cookies.Append("my-cookies", result.Value);
        
        return Ok();
    }


    [HttpPost("register")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Register(
        [FromServices] IMapper mapper,
        [FromBody] RegistrationRequest request, 
        CancellationToken ct)
    {
        var result = await service.Registration(mapper.Map<RegistrationCommand>(request), ct);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }


    [HttpPost("changepassword")]
    [Authorize]
    public Task<IActionResult> ChangePassword()
    {
        throw new NotImplementedException();
    }
}