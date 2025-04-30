using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Entities;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Storage.Repositories;

public class ImagesRepository(RShopDbContext dbContext, IMapper mapper) : IImagesRepository
{
    public async Task<bool> ImageExists(Guid id, CancellationToken ct)
    {
        return await dbContext.Images.AnyAsync(x => x.Id == id, ct);
    }

    public async Task<IEnumerable<ImageInfo>> GetImagesByProductId(Guid productId, CancellationToken ct)
    {
        return await dbContext.Images
            .Where(i => i.ProductId == productId)
            .ProjectTo<ImageInfo>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
    }

    public async Task DeleteImage(Guid id, CancellationToken ct)
    {
        await dbContext.Images.Where(i => i.Id == id).ExecuteDeleteAsync(ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task<Guid> AddImage(Guid productId, string imageExtension, CancellationToken ct)
    {
        var imageId = Guid.NewGuid();
        var image = new ImageInfoEntity()
        {
            Id = imageId,
            ProductId = productId,
            Extension = imageExtension
        };
        await dbContext.Images.AddAsync(image, ct);
        await dbContext.SaveChangesAsync(ct);
        return imageId;
    }
}