using Microsoft.AspNetCore.Mvc;
using RShopAPI_Test.DTOs;
using RShopAPI_Test.Services.Interfaces;

namespace RShopAPI_Test.Controllers;

[ApiController]
[Route("/api/categories")]
public class CategoryController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCategory(
        [FromServices] ICreateCategoryUseCase useCase, 
        CreateCategoryRequest request, 
        CancellationToken ct)
    {
        var category = await useCase.Handle(request.Name, ct);
        return Ok(category);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories(
        [FromServices] IGetCategoriesUseCase useCase, 
        CancellationToken ct)
    {
        var categories = await useCase.Handle(ct);
        return Ok(categories);
    }
}