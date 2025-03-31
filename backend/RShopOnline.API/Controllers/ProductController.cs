using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.DTOs;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Services.UseCases.CreateProduct;
using RShopAPI_Test.Services.UseCases.GetProduct;
using RShopAPI_Test.Services.UseCases.UpdateProduct;

namespace RShopAPI_Test.Controllers;

[ApiController]
[Route("/api/products")]
public class ProductController : ControllerBase
{
    [HttpGet] // TODO Pagination, filters and sorting
    [ProducesResponseType<List<Product>>(200)]
    public async Task<IActionResult> GetProducts([FromServices] IGetProductsUseCase useCase, CancellationToken ct)
    {
        var products = await useCase.Handle(ct);
        return Ok(products);
    }
    
    [HttpGet("{id::guid}")]
    [ProducesResponseType<Product>(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetProduct(
        [FromServices] IGetProductUseCase useCase, 
        [FromRoute] Guid id, 
        CancellationToken ct)
    {
        var product = await useCase.Handle(id, ct);

        if (product is null)
            return BadRequest();
        
        return Ok(product);
    }

    [HttpPost]
    [ProducesResponseType<Product>(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateProduct(
        [FromServices] IMapper mapper,
        [FromServices] ICreateProductUseCase useCase, 
        [FromBody] CreateProductRequest request, 
        CancellationToken ct)
    {
        var result = await useCase.Handle(mapper.Map<CreateProductCommand>(request), ct);
        
        if(result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpPut]
    [ProducesResponseType<Product>(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateProduct(
        [FromServices] IMapper mapper,
        [FromServices] IUpdateProductUseCase useCase, 
        [FromBody] UpdateProductRequest request, 
        CancellationToken ct)
    {
        var result = await useCase.Handle(mapper.Map<UpdateProductCommand>(request), ct);
        
        if(result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
}