using Microsoft.AspNetCore.Mvc;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.DTOs;
using RShopAPI_Test.Extensions;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Interfaces;

namespace RShopAPI_Test.Controllers;

[ApiController]
[Route("/api/categories")]
public class CategoryController(ICategoryService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<Category>(201)]
    public async Task<IActionResult> CreateCategory(
        CreateCategoryRequest request, 
        CancellationToken ct)
    {
        var result = await service.CreateCategory(new CreateCategoryCommand(request.Name), ct);
        return result.ToResponse();
    }

    [HttpGet]
    [ProducesResponseType<List<Category>>(200)]
    public async Task<IActionResult> GetAllCategories(
        CancellationToken ct)
    {
        var categories = await service.GetCategories(ct);
        return Ok(categories);
    }
}