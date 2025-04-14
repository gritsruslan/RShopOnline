using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Services;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopOnline.Tests.Services;

public class ProductServiceShould
{
    private ProductService sut;
    private Mock<IProductsRepository> productsRepository;
    private Mock<ICategoriesRepository> categoriesRepository;
    private ISetup<IProductsRepository, Task<Product>> createProductSetup;
    private ISetup<IProductsRepository, Task<Product>> updateProductSetup;
    private ISetup<ICategoriesRepository, Task<bool>> categoryExistsSetup;
    private ISetup<IProductsRepository, Task<bool>> productExistsSetup;
    private ISetup<IProductsRepository, Task<IEnumerable<Product>>> getProductsSetup;
    
    public ProductServiceShould()
    {
        categoriesRepository = new Mock<ICategoriesRepository>();
        categoryExistsSetup = categoriesRepository.Setup(
            c => c.CategoryExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        
        productsRepository = new Mock<IProductsRepository>();
        productExistsSetup = productsRepository
            .Setup(p => p.ProductExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        
        createProductSetup = productsRepository.Setup(p =>
            p.CreateProduct(It.IsAny<string>(), 
                It.IsAny<decimal>(), 
                It.IsAny<Guid>(), 
                It.IsAny<bool>(), 
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()));
        
        updateProductSetup = productsRepository.Setup(p => 
            p.UpdateProduct(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));
        
        getProductsSetup = productsRepository.Setup(
            p => p.GetProducts(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()));
        
        sut = new ProductService(productsRepository.Object, categoriesRepository.Object);
    }

    [Fact]
    public async Task ShouldCreateProduct_WhenCategoryExists()
    {
        Guid guid = Guid.Parse("9B4522C0-36F2-4387-BF29-230DF010EE78");
        string name = "name";
        decimal price = 100;
        var categoryId = Guid.Parse("1E5970AA-67B9-4E47-9D9D-C208D48F60D8");
        bool inStock = false;
        string description = "description";
        var expected = new Product()
        {
            Id = guid,
            Name = name,
            Price = price,
            InStock = inStock,
            Description = description
        };
        
        categoryExistsSetup.ReturnsAsync(true);
        createProductSetup.ReturnsAsync(expected);
        
        var command = new CreateProductCommand(name, price, categoryId, inStock, description);
        var result = await sut.CreateProduct(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);
    }

    [Fact]
    public async Task ShouldNotCreateProduct_WhenCategoryDoesNotExist()
    {
        categoryExistsSetup.ReturnsAsync(false);
        var command = new CreateProductCommand("name", 100, Guid.Parse("82960DFD-44ED-4B7B-9B00-15C556E80824"), true, "description");
        var result = await sut.CreateProduct(command, CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldNotUpdateProduct_WhenCategoryDoesNotExist()
    {
        productExistsSetup.ReturnsAsync(true);
        categoryExistsSetup.ReturnsAsync(false);
        
        var id = Guid.Parse("A7F5C976-90B5-4F78-84EF-4B3DA84F81C4");
        var categoryId = Guid.Parse("563F18DC-6B6E-4E44-8EC4-84311CE2DB5A");
        var command = new UpdateProductCommand(id, "name", 100, categoryId, true, "description");
        var result = await sut.UpdateProduct(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeFalse();
    }
    
    [Fact]
    public async Task ShouldNotUpdateProduct_WhenProductDoesNotExist()
    {
        productExistsSetup.ReturnsAsync(false);
        categoryExistsSetup.ReturnsAsync(true);

        var id = Guid.Parse("A7F5C976-90B5-4F78-84EF-4B3DA84F81C4");
        var categoryId = Guid.Parse("563F18DC-6B6E-4E44-8EC4-84311CE2DB5A");
        var command = new UpdateProductCommand(id, "name", 100, categoryId, true, "description");
        var result = await sut.UpdateProduct(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldCorrectUpdateProduct_WhenProductAndCategoryExists()
    {
        Guid id = Guid.Parse("A9AF5CAD-3163-4FF4-8072-64EABB807CE2");
        Guid categoryId = Guid.Parse("89519283-256B-4EFB-84A2-5403FF620397");
        string name = "name";
        decimal price = 100;
        bool inStock = false;
        string description = "description";
        var expected = new Product()
        {
            Id = id,
            Name = name,
            Price = price,
            InStock = inStock,
            Description = description
        };
        
        productExistsSetup.ReturnsAsync(true);
        categoryExistsSetup.ReturnsAsync(true);
        updateProductSetup.ReturnsAsync(expected);
        
        var command  = new UpdateProductCommand(id, name, price, categoryId, inStock, description);
        var result = await sut.UpdateProduct(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);
    }

    [Fact]
    public async Task ShouldNotGetProductsWithPagination_WhenCategoryDoesNotExist()
    {
        categoryExistsSetup.ReturnsAsync(false);

        var categoryId = Guid.Parse("6A1C1C2C-30E3-48EE-802C-27C3D83C1250");
        var command = new GetProductsCommand(categoryId, 5, 10, "Price", true);

        var result = await sut.GetProducts(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldNotGetProductsWithPagination_WhenIncorrectOrderField()
    {
        categoryExistsSetup.ReturnsAsync(true);
        
        var categoryId = Guid.Parse("6A1C1C2C-30E3-48EE-802C-27C3D83C1250");
        var command = new GetProductsCommand(categoryId, 5, 10, "Price222", true);

        var result = await sut.GetProducts(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldNotGetProductsWithPagination_WhenPageIsIncorrect()
    {
        categoryExistsSetup.ReturnsAsync(true);
        
        var categoryId = Guid.Parse("6A1C1C2C-30E3-48EE-802C-27C3D83C1250");
        var command = new GetProductsCommand(categoryId, -1488, 10, "Price", true);

        var result = await sut.GetProducts(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeFalse();
    }
    
    [Fact]
    public async Task ShouldNotGetProductsWithPagination_WhenPageSizeIsIncorrect()
    {
        categoryExistsSetup.ReturnsAsync(true);
        
        var categoryId = Guid.Parse("6A1C1C2C-30E3-48EE-802C-27C3D83C1250");
        var command = new GetProductsCommand(categoryId, 5, -1488, "Price", true);

        var result = await sut.GetProducts(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldCorrectReturnProductsWithPagination()
    {
        Product[] expected = [];
        
        categoryExistsSetup.ReturnsAsync(true);
        getProductsSetup.ReturnsAsync(expected);
        
        var categoryId = Guid.Parse("6A1C1C2C-30E3-48EE-802C-27C3D83C1250");
        var command = new GetProductsCommand(categoryId, 5, 10, "Price", true);

        var result = await sut.GetProducts(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expected);
    }
}