using FluentAssertions;
using RShopAPI_Test.Storage.Entities;
using RShopAPI_Test.Storage.Repositories;

namespace RShopOnline.Storage.Tests;

public class ProductsRepositoryFixture : StorageTestFixture
{
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await using var dbContext = GetDbContext();

        var categoryId = Guid.Parse("0A5018A3-81AF-4EBE-A3F4-B15C94C2CFA5");
        await dbContext.Categories.AddRangeAsync(new CategoryEntity()
        {
            Id = categoryId,
            Name = "Category"
        }, new CategoryEntity()
        {
            Id = Guid.Parse("A170E0F4-101D-49BF-8250-CF072597A620"),
            Name = "Category2"
        });

        await dbContext.Products.AddRangeAsync(new ProductEntity
        {
            Id = Guid.Parse("CF7C50A6-C816-4B70-B092-E4552783CBFB"),
            Name = "p1",
            Price = 1000,
            Description = "p1 description",
            CategoryId = categoryId,
            InStock = true,
        },new ProductEntity
        {
            Id = Guid.Parse("B7442D60-C243-485B-B501-F43FAFE4F49D"),
            Name = "p2",
            Price = 2000,
            Description = "p2 description",
            CategoryId = categoryId,
            InStock = true,
        },new ProductEntity
        {
            Id = Guid.Parse("7A79FDCF-C4CE-4A97-816A-C25485CCF907"),
            Name = "p3",
            Price = 3000,
            Description = "p3 description",
            CategoryId = categoryId,
            InStock = true,
        });

        await dbContext.SaveChangesAsync();
    }
}


public class ProductsRepositoryShould(ProductsRepositoryFixture fixture) : IClassFixture<ProductsRepositoryFixture>
{
    private readonly ProductsRepository Sut = new(fixture.GetDbContext(), fixture.GetMapper());

    [Fact]
    public async Task CreateProductAndGetProductById()
    {
        var product = await Sut.CreateProduct("product", 
            1488, Guid.Parse("A170E0F4-101D-49BF-8250-CF072597A620"), 
            false, "description", CancellationToken.None );
        
        var getProduct = await Sut.GetProductById(product.Id, CancellationToken.None);
        getProduct.Should().NotBeNull();
        getProduct.Id.Should().Be(product.Id);
    }


    [Fact]
    public async Task GetProductsUsingPaginationAndFiltersAndOrder()
    {
        var products = await Sut.GetProducts(
            Guid.Parse("0A5018A3-81AF-4EBE-A3F4-B15C94C2CFA5"), 1, 2, "Price", true, 
            CancellationToken.None);
        var productsList = products.ToList();
        productsList.Should().HaveCount(2);
        productsList[0].Id.Should().Be(Guid.Parse("B7442D60-C243-485B-B501-F43FAFE4F49D"));
        productsList[1].Id.Should().Be(Guid.Parse("7A79FDCF-C4CE-4A97-816A-C25485CCF907"));
    }

    [Fact]
    public async Task UpdateProduct()
    {
        var productId = Guid.Parse("CF7C50A6-C816-4B70-B092-E4552783CBFB");
        var newName = "newName";
        decimal newPrice = 1488;
        bool newInStock = false;
        string newDescription = "newDescription";
        
        await Sut.UpdateProduct(productId, newName, newPrice, newInStock, newDescription, CancellationToken.None);
        var getProduct = await Sut.GetProductById(productId, CancellationToken.None);
        
        getProduct.Should().NotBeNull();
        getProduct.Name.Should().Be(newName);
        getProduct.Price.Should().Be(newPrice);
        getProduct.InStock.Should().Be(newInStock);
        getProduct.Description.Should().Be(newDescription);
    }
}