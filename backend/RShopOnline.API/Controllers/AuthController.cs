using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RShopAPI_Test.DTOs;
using RShopAPI_Test.Services.UseCases.Registration;

namespace RShopAPI_Test.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase 
{
    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        throw new NotImplementedException();
    }


    [HttpPost("register")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Register(
        [FromServices] IRegistrationUseCase useCase,
        [FromServices] IMapper mapper,
        [FromBody] RegistrationRequest request, 
        CancellationToken ct)
    {
        var result = await useCase.Handle(mapper.Map<RegistrationCommand>(request), ct);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }


    [HttpPost("changepassword")]
    public async Task<IActionResult> ChangePassword()
    {
        throw new NotImplementedException();
    }
}