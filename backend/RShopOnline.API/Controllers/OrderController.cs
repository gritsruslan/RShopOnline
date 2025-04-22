using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.DTOs;
using RShopAPI_Test.Factories;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Interfaces;

namespace RShopAPI_Test.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController(IOrderService service, IMapper mapper) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType<Order>(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetOrderById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await service.GetOrderById(new(id), ct);
        return HttpResponseFactory.FromResult(result);
    }

    [HttpGet("user")]
    [ProducesResponseType<List<Order>>(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetOrdersByCurrentUser(CancellationToken ct)
    {
        var orders = await service.GetOrdersByCurrentUser(new(),ct);
        return Ok(orders);
    }
    
    [HttpPost]
    [ProducesResponseType<Order>(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken ct)
    {
        var result = await service.CreateOrder(mapper.Map<CreateOrderCommand>(request), ct);
        return HttpResponseFactory.FromResult(result);
    }

    [HttpPut("cancel")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CancelOrder([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await service.CancelOrder(new(id), ct);
        return HttpResponseFactory.FromResult(result);
    }

    [HttpPut]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request, CancellationToken ct)
    {
        var result = await service.UpdateOrderStatus(mapper.Map<UpdateOrderStatusCommand>(request), ct);
        return HttpResponseFactory.FromResult(result);
    }
}