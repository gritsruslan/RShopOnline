using System.Net.Mime;
using RShopAPI_Test.Core.Common;

namespace RShopAPI_Test.Services.Interfaces;

public interface IImageService
{
    Task<Result<(Stream stream, ContentType contentType)>> GetImage(string name, CancellationToken ct);
}