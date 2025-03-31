namespace RShopAPI_Test.Storage.Entities;

public class CategoryEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<ProductEntity> Products { get; set; } = null!;
}