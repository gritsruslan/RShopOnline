using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Storage.Interfaces;

public interface IImagesRepository
{
    Task<bool> ImageExists(Guid id, CancellationToken ct);
    
    Task<IEnumerable<ImageInfo>> GetImagesByProductId(Guid productId, CancellationToken ct);
    
    Task DeleteImage(Guid id, CancellationToken ct);
    
    Task<Guid> AddImage(Guid productId, string imageExtension, CancellationToken ct);
}