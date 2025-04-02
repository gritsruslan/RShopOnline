namespace RShopAPI_Test.Storage.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public decimal Price { get; set; }
    
    public bool InStock { get; set; }
    
    public string Description { get; set; } = null!;
    
    public Guid CategoryId { get; set; }
    
    public CategoryEntity Category { get; set; } = null!;
    
    public ICollection<OrderEntity> Orders { get; set; } = null!;
}