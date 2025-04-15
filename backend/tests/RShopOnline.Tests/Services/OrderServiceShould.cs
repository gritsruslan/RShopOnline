using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Services.Services;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopOnline.Tests.Services;

public class OrderServiceShould
{
    
    private readonly IOrderService Sut;
    private readonly ISetup<ICurrentUserService, Guid?> GetCurrentUserSetup;
    private readonly ISetup<IProductsRepository, Task<Product?>> GetProductByIdSetup;
    private readonly ISetup<IOrdersRepository, Task<Order?>> GetOrderByIdSetup;

    public OrderServiceShould()
    {
        var ordersRepository = new Mock<IOrdersRepository>();
        var productsRepository = new Mock<IProductsRepository>();
        var currentUserService = new Mock<ICurrentUserService>();
        
        Sut = new OrderService(ordersRepository.Object, productsRepository.Object, currentUserService.Object);

        GetCurrentUserSetup = currentUserService.Setup(s => s.GetCurrentUserId());

        GetProductByIdSetup =
            productsRepository.Setup(s => s.GetProductById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        
        GetOrderByIdSetup = ordersRepository.Setup(s => s.GetOrderById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
    }
    
    [Fact]
    public async Task ThrownUnauthorizedExceptionWhileCreatingOrder_WhenUserIsNotAuthenticated()
    {
        GetCurrentUserSetup.Returns((Guid?)null);
        
        await Sut.Invoking(s => s.CreateOrder(new CreateOrderCommand([]), CancellationToken.None)).Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task DontCreateOrder_WhenOrderIsEmpty()
    {
        GetCurrentUserSetup.Returns(Guid.Parse("9123022D-3749-4374-A30C-9F81EB963298"));
        
        var result = await Sut.CreateOrder(new CreateOrderCommand([]), CancellationToken.None);
        
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DontCreateOrder_WhenInvalidQuantityOfProduct()
    {
        GetCurrentUserSetup.Returns(Guid.Parse("9123022D-3749-4374-A30C-9F81EB963298"));
        var orderItem = new OrderItemDto(Guid.Parse("729A272D-BC7F-4A99-A148-19EA0B1B358D"), -10);

        var result = await Sut.CreateOrder(new CreateOrderCommand([orderItem]), CancellationToken.None);
        
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DontCreateOrder_WhenProductNotFound()
    {
        GetCurrentUserSetup.Returns(Guid.Parse("9123022D-3749-4374-A30C-9F81EB963298"));
        GetProductByIdSetup.ReturnsAsync((Product?)null);
        var orderItem = new OrderItemDto(Guid.Parse("729A272D-BC7F-4A99-A148-19EA0B1B358D"), 1);
        
        var result = await Sut.CreateOrder(new CreateOrderCommand([orderItem]), CancellationToken.None);
        
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DontCreateOrder_WhenProductIsNotInStock()
    {
        GetCurrentUserSetup.Returns(Guid.Parse("9123022D-3749-4374-A30C-9F81EB963298"));
        GetProductByIdSetup.ReturnsAsync(new Product()
        {
            Name = "name",
            Description = "description",
            InStock = false
        });
        
        var orderItem = new OrderItemDto(Guid.Parse("729A272D-BC7F-4A99-A148-19EA0B1B358D"), 1);
        
        var result = await Sut.CreateOrder(new CreateOrderCommand([orderItem]), CancellationToken.None);
        
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DontCreateOrder_WhenProductAddedMultipleTimes()
    {
        GetCurrentUserSetup.Returns(Guid.Parse("9123022D-3749-4374-A30C-9F81EB963298"));
        var productId = Guid.Parse("729A272D-BC7F-4A99-A148-19EA0B1B358D");
        GetProductByIdSetup.ReturnsAsync(new Product()
        {
            Id = productId,
            Name = "name",
            Description = "description",
            InStock = true
        });
        
        var orderItem = new OrderItemDto(productId, 1);
        var orderItem2 = new OrderItemDto(productId, 3);
        
        var result = await Sut.CreateOrder(new CreateOrderCommand([orderItem, orderItem2]), CancellationToken.None);
        
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task SuccessfullyCreateOrder()
    {
        GetCurrentUserSetup.Returns(Guid.Parse("9123022D-3749-4374-A30C-9F81EB963298"));
        GetProductByIdSetup.ReturnsAsync(new Product()
        {
            Name = "name",
            Description = "description",
            InStock = true
        });
        
        var orderItem = new OrderItemDto(Guid.Parse("729A272D-BC7F-4A99-A148-19EA0B1B358D"), 1);
        
        var result = await Sut.CreateOrder(new CreateOrderCommand([orderItem]), CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ThrowUnauthorizedExceptionWhileCancelingOrder_WhenUserIsNotAuthenticated()
    {
        GetCurrentUserSetup.Returns((Guid?)null);

        await Sut.Invoking(s =>
                s.CancelOrder(Guid.Parse("54364834-1E03-4688-912B-2D3883880FDD"), CancellationToken.None))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task DontCancelOrder_WhenOrderNotFound()
    {
        GetCurrentUserSetup.Returns(Guid.Parse("9123022D-3749-4374-A30C-9F81EB963298"));
        GetOrderByIdSetup.ReturnsAsync((Order?)null);

        var result = await Sut.CancelOrder(Guid.Parse("4D93A32F-9DFC-424A-A23B-32DA66317F1F"), CancellationToken.None);
        
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DontCancelOrder_WhenUserIsNotACreatorOfOrder()
    {
        GetCurrentUserSetup.Returns(Guid.Parse("9123022D-3749-4374-A30C-9F81EB963298"));
        GetOrderByIdSetup.ReturnsAsync(new Order
        {
            UserId = Guid.Parse("E861A307-1E93-4BAC-9D07-083A4B4053C1"),
            OrderItems = []
        });
        
        var result = await Sut.CancelOrder(Guid.Parse("65AF87D3-6EEB-4333-858B-CF30F50CA49D"), CancellationToken.None);
        
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DontCancelOrder_WhenOrderStatusIsNotPending()
    {
        var userId = Guid.Parse("E861A307-1E93-4BAC-9D07-083A4B4053C1");
        GetCurrentUserSetup.Returns(userId);
        GetOrderByIdSetup.ReturnsAsync(new Order
        {
            UserId = userId,
            OrderItems = [],
            Status = OrderStatus.Sent
        });

        var result = await Sut.CancelOrder(Guid.Parse("65AF87D3-6EEB-4333-858B-CF30F50CA49D"), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task SuccessfullyCancelOrder()
    {
        var userId = Guid.Parse("E861A307-1E93-4BAC-9D07-083A4B4053C1");
        GetCurrentUserSetup.Returns(userId);
        GetOrderByIdSetup.ReturnsAsync(new Order
        {
            UserId = userId,
            OrderItems = [],
            Status = OrderStatus.Pending
        });

        var result = await Sut.CancelOrder(Guid.Parse("65AF87D3-6EEB-4333-858B-CF30F50CA49D"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DontUpdateOrderStatus_WhenOrderNotFound()
    {
        GetOrderByIdSetup.ReturnsAsync((Order?)null);

        var result = await Sut.UpdateOrderStatus(
            new UpdateOrderStatusCommand(Guid.Parse("65AF87D3-6EEB-4333-858B-CF30F50CA49D"), OrderStatus.Completed), CancellationToken.None);
        
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task SuccessfullyUpdateOrderStatus()
    {
        GetOrderByIdSetup.ReturnsAsync(new Order
        {
            OrderItems = [],
            Status = OrderStatus.Pending
        });

        var result = await Sut.UpdateOrderStatus(
            new UpdateOrderStatusCommand(Guid.Parse("65AF87D3-6EEB-4333-858B-CF30F50CA49D"), OrderStatus.Completed), CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
    }
}