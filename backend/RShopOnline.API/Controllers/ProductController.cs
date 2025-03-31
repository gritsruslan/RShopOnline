using Microsoft.AspNetCore.Mvc;
using RShopAPI_Test.DTOs;

namespace RShopAPI_Test.Controllers;

[ApiController]
[Route("/api/products")]
public class ProductController : ControllerBase
{
    [HttpGet] // TODO Pagination, filters and sorting
    public async Task<IActionResult> GetProducts()
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("{id::guid}")]
    public async Task<IActionResult> GetProduct([FromQuery] Guid id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}