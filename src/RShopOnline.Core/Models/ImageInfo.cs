namespace RShopAPI_Test.Core.Models;

public class ImageInfo
{
    public Guid Id { get; set; }
    
    public Guid ProductId { get; set; }

    public string Extension { get; set; } = null!;
}