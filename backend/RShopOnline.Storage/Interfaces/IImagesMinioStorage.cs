using System.Net.Mime;

namespace RShopAPI_Test.Storage.Interfaces;

public interface IImagesMinioStorage
{
    Task<bool> ImageExists(string imageName, CancellationToken ct);

    Task<(Stream, ContentType)> GetImage(string imageName, CancellationToken ct);
        
    Task UploadImage(Stream imageStream, string imageName, string contentType, long objectSize, CancellationToken ct);
    
    Task DeleteImage(string imageName, CancellationToken ct);
}