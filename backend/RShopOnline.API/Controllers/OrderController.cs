using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.DTOs;
using RShopAPI_Test.Services.Auth;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Interfaces;

namespace RShopAPI_Test.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController(IOrderService service, IMapper mapper) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await service.GetOrderById(id, ct);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }

    [HttpGet("user")]
    [RequireRole(Role.Customer)]
    public async Task<IActionResult> GetOrdersByCurrentUser(CancellationToken ct)
    {
        var orders = await service.GetOrdersByCurrentUser(ct);
        return Ok(orders);
    }
    
    [HttpPost]
    [RequireRole(Role.Customer)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken ct)
    {
        var result = await service.CreateOrder(mapper.Map<CreateOrderCommand>(request), ct);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }

    [HttpPut("cancel")]
    [RequireRole(Role.Customer)]
    public async Task<IActionResult> CancelOrder([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await service.CancelOrder(id, ct);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok();
    }

    [HttpPut]
    [RequireRole(Role.Manager, Role.Admin)]
    public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request, CancellationToken ct)
    {
        var result = await service.UpdateOrderStatus(mapper.Map<UpdateOrderStatusCommand>(request), ct);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok();
    }
}