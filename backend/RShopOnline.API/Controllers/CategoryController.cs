using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.DTOs;
using RShopAPI_Test.Services.Auth;
using RShopAPI_Test.Services.Interfaces;

namespace RShopAPI_Test.Controllers;

[ApiController]
[Route("/api/categories")]
public class CategoryController(ICategoryService service) : ControllerBase
{
    [HttpPost]
    [RequireRole(Role.Admin, Role.Manager)]
    [ProducesResponseType<Category>(201)]
    public async Task<IActionResult> CreateCategory(
        CreateCategoryRequest request, 
        CancellationToken ct)
    {
        await service.CreateCategory(request.Name, ct);
        return Ok();
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