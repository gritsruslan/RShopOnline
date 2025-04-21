using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.DTOs;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Interfaces;

namespace RShopAPI_Test.Controllers;

[ApiController]
[Route("/api/products")]
public class ProductController(IProductService service) : ControllerBase
{
    [HttpGet("all")]
    [ProducesResponseType<List<Product>>(200)]
    public async Task<IActionResult> GetAllProducts(
        CancellationToken ct)
    {
        var products = await service.GetAllProducts(ct);
        return Ok(products);
    }

    [HttpGet]
    [ProducesResponseType<List<Product>>( 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetProducts(
        [FromServices] IMapper mapper,
        [FromQuery] Guid categoryId,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] string orderBy,
        [FromQuery] bool ascending,
        CancellationToken ct)
    {
        var getProductsCommand = new GetProductsCommand(categoryId, page, pageSize, orderBy, ascending);
        var result = await service.GetProducts(getProductsCommand, ct);
        
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
    [HttpGet("{id::guid}")]
    [ProducesResponseType<Product>(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetProduct(
        [FromRoute] Guid id, 
        CancellationToken ct)
    {
        var result = await service.GetProduct(id, ct);

        if (result.IsFailure)
            return BadRequest();
        
        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType<Product>(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateProduct(
        [FromServices] IMapper mapper,
        [FromBody] CreateProductRequest request, 
        CancellationToken ct)
    {
        var result = await service.CreateProduct(mapper.Map<CreateProductCommand>(request), ct);
        
        if(result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpPut]
    [ProducesResponseType<Product>(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateProduct(
        [FromServices] IMapper mapper,
        [FromBody] UpdateProductRequest request, 
        CancellationToken ct)
    {
        var result = await service.UpdateProduct(mapper.Map<UpdateProductCommand>(request), ct);
        
        if(result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
}