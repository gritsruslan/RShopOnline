namespace RShopAPI_Test.Storage.Entities;

public class ImageInfoEntity
{
    public Guid Id { get; set; }

    public string Extension { get; set; } = null!;
    
    public Guid ProductId { get; set; }
    
    public ProductEntity Product { get; set; } = null!;
}