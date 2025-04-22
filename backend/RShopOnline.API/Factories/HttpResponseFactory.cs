using Microsoft.AspNetCore.Mvc;
using RShopAPI_Test.Core.Common;
using EmptyResult = RShopAPI_Test.Core.Common.EmptyResult;

namespace RShopAPI_Test.Factories;

public static class HttpResponseFactory
{
    public static IActionResult FromResult(EmptyResult result)
    {
        if (result.IsSuccess)
        {
            return result.SuccessCode switch
            {
                SuccessCode.Ok => new OkResult(),
                SuccessCode.Created => new CreatedResult(),
                SuccessCode.NoContent => new NoContentResult(),
                _ => throw new ArgumentOutOfRangeException(nameof(result.SuccessCode), "Unexpected success code!")
            };
        }
        
        return result.ErrorCode switch
        {
            ErrorCode.Unauthorized => new UnauthorizedResult(),
            ErrorCode.Forbidden => new ForbidResult(),
            ErrorCode.NotFound => new NotFoundObjectResult(result.Error),
            ErrorCode.BadRequest => new BadRequestObjectResult(result.Error),
            _ => throw new ArgumentOutOfRangeException(nameof(result.ErrorCode), "Unexpected error code!")
        };
    }

    public static IActionResult FromResult<T>(Result<T> result)
    {
        
        if (result.IsSuccess)
        {
            return result.SuccessCode switch
            {
                SuccessCode.Ok => new OkObjectResult(result.Value),
                SuccessCode.Created => new CreatedResult(string.Empty, result.Value),
                SuccessCode.NoContent => new NoContentResult(), // must be unreachable
                _ => throw new ArgumentOutOfRangeException(nameof(result.SuccessCode), "Unexpected success code!")
            };
        }
        
        return result.ErrorCode switch
        {
            ErrorCode.Unauthorized => new UnauthorizedResult(),
            ErrorCode.Forbidden => new ForbidResult(),
            ErrorCode.NotFound => new NotFoundObjectResult(result.Error),
            ErrorCode.BadRequest => new BadRequestObjectResult(result.Error),
            _ => throw new ArgumentOutOfRangeException(nameof(result.ErrorCode), "Unexpected error code!")
        };
    }
}