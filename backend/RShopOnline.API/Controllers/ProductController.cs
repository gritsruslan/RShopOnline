using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.DTOs;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.UseCases.CreateProduct;
using RShopAPI_Test.Services.UseCases.GetProduct;
using RShopAPI_Test.Services.UseCases.GetProducts;
using RShopAPI_Test.Services.UseCases.UpdateProduct;

namespace RShopAPI_Test.Controllers;

[ApiController]
[Route("/api/products")]
public class ProductController : ControllerBase
{
    [HttpGet("all")]
    [ProducesResponseType<List<Product>>(200)]
    public async Task<IActionResult> GetAllProducts([FromServices] IGetAllProductsUseCase useCase, CancellationToken ct)
    {
        var products = await useCase.Handle(ct);
        return Ok(products);
    }

    [HttpGet]
    [ProducesResponseType<List<Product>>( 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetProducts(
        [FromServices] IGetProductsUseCase useCase,
        [FromServices] IMapper mapper,
        [FromQuery] Guid categoryId,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] string orderBy,
        [FromQuery] bool ascending,
        CancellationToken ct)
    {
        var getProductsCommand = new GetProductsCommand(categoryId, page, pageSize, orderBy, ascending);
        var result = await useCase.Handle(getProductsCommand, ct);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
    [HttpGet("{id::guid}")]
    [ProducesResponseType<Product>(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetProduct(
        [FromServices] IGetProductUseCase useCase, 
        [FromRoute] Guid id, 
        CancellationToken ct)
    {
        var result = await useCase.Handle(id, ct);

        if (result.IsFailure)
            return BadRequest();
        
        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType<Product>(201)]
    [ProducesResponseType(404)]
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
    [ProducesResponseType<Product>(204)]
    [ProducesResponseType(404)]
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