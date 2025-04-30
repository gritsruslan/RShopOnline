using Microsoft.AspNetCore.Mvc;
using RShopAPI_Test.Services.Interfaces;

namespace RShopAPI_Test.Controllers;

[ApiController]
[Route("api/images")]
public class ImageController(IImageService service) : ControllerBase
{
    [HttpGet("{fileName}")]
    public async Task<IActionResult> GetImage(
        [FromRoute] string fileName, 
        CancellationToken ct)
    {
        var result = await service.GetImage(fileName, ct);
        if (result.IsFailure)
        {
            return BadRequest();
        }
        
        var (stream, contentType) = result.Value;
        return File(stream, contentType.ToString(), fileName);
    }
}