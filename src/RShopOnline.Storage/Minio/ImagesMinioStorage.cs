using System.Net.Mime;
using Minio;
using Minio.DataModel.Args;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Storage.Minio;

public class ImagesMinioStorage(IMinioClient minioClient) : IImagesMinioStorage
{
    public async Task<bool> ImageExists(string imageName, CancellationToken ct)
    {
        await EnsureBucketExists(ct);
        var statObject = await minioClient.StatObjectAsync(
            new StatObjectArgs().WithBucket(MinioBuckets.ImagesBucketName).WithObject(imageName), ct);
        return statObject.Size != 0;
    }

    public async Task<(Stream, ContentType)> GetImage(string imageName, CancellationToken ct)
    {
        var fileStat = await minioClient.StatObjectAsync(
            new StatObjectArgs().WithBucket(MinioBuckets.ImagesBucketName).WithObject(imageName), ct);
        
        var memoryStream = new MemoryStream();
        await minioClient.GetObjectAsync(
            new GetObjectArgs().WithBucket(MinioBuckets.ImagesBucketName).WithObject(imageName)
                .WithCallbackStream(stream => { stream.CopyTo(memoryStream); }), ct);
        
        var contentType = new ContentType(fileStat.ContentType);
        memoryStream.Position = 0;
        return (memoryStream, contentType);
    }

    public async Task UploadImage(
        Stream imageStream, 
        string imageName, 
        string contentType, 
        long objectSize, 
        CancellationToken ct)
    {
        await EnsureBucketExists(ct);
        imageStream.Position = 0;
        
        await minioClient.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(MinioBuckets.ImagesBucketName)
                .WithObject(imageName)
                .WithStreamData(imageStream)
                .WithObjectSize(objectSize)
                .WithContentType(contentType), ct);
    }

    public async Task DeleteImage(string imageName , CancellationToken ct)
    {
        await minioClient.RemoveObjectAsync(
            new RemoveObjectArgs()
                .WithBucket(MinioBuckets.ImagesBucketName)
                .WithObject(imageName), ct);
    }

    private async Task EnsureBucketExists(CancellationToken ct)
    {
        bool bucketExists = await minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(MinioBuckets.ImagesBucketName), ct);
        if (!bucketExists)
        {
            await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(MinioBuckets.ImagesBucketName), ct);
        }
    }
}