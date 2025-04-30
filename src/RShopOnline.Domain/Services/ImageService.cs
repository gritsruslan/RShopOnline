using System.Net.Mime;
using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.Services;

public class ImageService(IImagesMinioStorage minioStorage) : IImageService
{
    public async Task<Result<(Stream, ContentType)>> GetImage(string name, CancellationToken ct)
    {
        var (stream, contentType) = await minioStorage.GetImage(name, ct);
        return (stream, contentType);
    }
}