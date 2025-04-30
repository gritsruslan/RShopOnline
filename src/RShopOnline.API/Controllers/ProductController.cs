using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.DTOs;
using RShopAPI_Test.Extensions;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Interfaces;

namespace RShopAPI_Test.Controllers;

[ApiController]
[Route("api/products")]
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
        return result.ToResponse();
    }
    
    [HttpGet("{id::guid}")]
    [ProducesResponseType<Product>(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetProduct(
        [FromRoute] Guid id, 
        CancellationToken ct)
    {
        var result = await service.GetProduct(id, ct);
        return result.ToResponse();
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
        return result.ToResponse();
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
        return result.ToResponse();
    }
    
    
    //Images

    [HttpGet("{id::guid}/images")]
    public async Task<IActionResult> GetProductImagesNames(
        [FromRoute] Guid id, 
        CancellationToken ct)
    {
        var result = await service.GetProductImagesNames(id, ct);
        return result.ToResponse();
    }

    [HttpPost("{id::guid}/images")]
    public async Task<IActionResult> AddProductImage(
        [FromRoute] Guid id,
        IFormFile file,
        CancellationToken ct)
    {
        var result = await service.AddProductImage(new AddProductImageCommand(file, id), ct);
        return result.ToResponse();
    }

    [HttpDelete("{id::guid}/images/{imageName}")]
    public async Task<IActionResult> DeleteProductImage(
        [FromRoute] Guid id,
        [FromRoute] string imageName,
        CancellationToken ct)
    {
        var result = await service.DeleteProductImage(new DeleteProductImageCommand(id, imageName), ct);
        return result.ToResponse();
    }
}