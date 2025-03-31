using Microsoft.AspNetCore.Mvc;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.DTOs;
using RShopAPI_Test.Services.UseCases.CreateCategory;
using RShopAPI_Test.Services.UseCases.GetCatigoriesUseCase;

namespace RShopAPI_Test.Controllers;

[ApiController]
[Route("/api/categories")]
public class CategoryController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<Category>(201)]
    public async Task<IActionResult> CreateCategory(
        [FromServices] ICreateCategoryUseCase useCase, 
        CreateCategoryRequest request, 
        CancellationToken ct)
    {
        var category = await useCase.Handle(request.Name, ct);
        return Ok(category);
    }

    [HttpGet]
    [ProducesResponseType<List<Category>>(200)]
    public async Task<IActionResult> GetAllCategories(
        [FromServices] IGetCategoriesUseCase useCase, 
        CancellationToken ct)
    {
        var categories = await useCase.Handle(ct);
        return Ok(categories);
    }
}