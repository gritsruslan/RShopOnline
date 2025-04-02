namespace RShopAPI_Test.Core.Models;

public class Product
{
    public Guid Id { get; set; }

    public required string Name { get; set; }
    
    public decimal Price { get; set; }
    
    public bool InStock { get; set; }

    public required string Description { get; set; }
}