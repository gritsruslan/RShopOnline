using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Moq;
using Moq.Language.Flow;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Authentication;
using RShopAPI_Test.Services.Authorization;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Services.Services;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopOnline.Domain.Tests.Services;

public class OrderServiceShould
{
    
    private readonly IOrderService Sut;
    private readonly ISetup<IProductsRepository, Task<Product?>> GetProductByIdSetup;
    private readonly ISetup<IOrdersRepository, Task<Order?>> GetOrderByIdSetup;
    private readonly ISetup<IIdentityProvider, IIdentity> GetCurrentIdentitySetup;


    public OrderServiceShould()
    {
        var ordersRepository = new Mock<IOrdersRepository>();
        var productsRepository = new Mock<IProductsRepository>();
        var identityProvider = new Mock<IIdentityProvider>();
        var intentionManager = new Mock<IIntentionManager>();
        intentionManager.Setup(m => m.IsAllowed<GetOrderByIdCommand>()).Returns(true);
        intentionManager.Setup(m => m.IsAllowed<GetOrdersByCurrentUserCommand>()).Returns(true);
        intentionManager.Setup(m => m.IsAllowed<CreateOrderCommand>()).Returns(true);
        intentionManager.Setup(m => m.IsAllowed<CancelOrderCommand>()).Returns(true);
        intentionManager.Setup(m => m.IsAllowed<UpdateOrderStatusCommand>()).Returns(true);
        
        Sut = new OrderService(
            ordersRepository.Object, 
            productsRepository.Object, 
            identityProvider.Object, 
            intentionManager.Object);
        
        GetProductByIdSetup =
            productsRepository.Setup(s => s.GetProductById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        
        GetOrderByIdSetup = ordersRepository.Setup(s => s.GetOrderById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        GetCurrentIdentitySetup = identityProvider.Setup(r => r.Current);
    }
    
    [Fact]
    public async Task DontCreateOrder_WhenOrderIsEmpty()
    {
        GetCurrentIdentitySetup.Returns(new RecognizedUser(Guid.Parse("BFD817A2-940D-4CE6-B8F5-675E55AE4768"),
            Role.Customer));
        var result = await Sut.CreateOrder(new CreateOrderCommand([]), CancellationToken.None);
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DontCreateOrder_WhenInvalidQuantityOfProduct()
    {
        GetCurrentIdentitySetup.Returns(new RecognizedUser(Guid.Parse("BFD817A2-940D-4CE6-B8F5-675E55AE4768"),
            Role.Customer));
        var orderItem = new OrderItemDto(Guid.Parse("729A272D-BC7F-4A99-A148-19EA0B1B358D"), -10);

        var result = await Sut.CreateOrder(new CreateOrderCommand([orderItem]), CancellationToken.None);
        
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DontCreateOrder_WhenProductNotFound()
    {
        GetCurrentIdentitySetup.Returns(new RecognizedUser(Guid.Parse("BFD817A2-940D-4CE6-B8F5-675E55AE4768"),
            Role.Customer));
        GetProductByIdSetup.ReturnsAsync((Product?)null);
        var orderItem = new OrderItemDto(Guid.Parse("729A272D-BC7F-4A99-A148-19EA0B1B358D"), 1);
        
        var result = await Sut.CreateOrder(new CreateOrderCommand([orderItem]), CancellationToken.None);
        
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DontCreateOrder_WhenProductIsNotInStock()
    {
        GetCurrentIdentitySetup.Returns(new RecognizedUser(Guid.Parse("BFD817A2-940D-4CE6-B8F5-675E55AE4768"),
            Role.Customer));
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
        GetCurrentIdentitySetup.Returns(new RecognizedUser(Guid.Parse("BFD817A2-940D-4CE6-B8F5-675E55AE4768"),
            Role.Customer));
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
        GetCurrentIdentitySetup.Returns(new RecognizedUser(Guid.Parse("BFD817A2-940D-4CE6-B8F5-675E55AE4768"),
            Role.Customer));
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
    public async Task DontCancelOrder_WhenOrderNotFound()
    {
        GetCurrentIdentitySetup.Returns(new RecognizedUser(Guid.Parse("BFD817A2-940D-4CE6-B8F5-675E55AE4768"),
            Role.Customer));
        GetOrderByIdSetup.ReturnsAsync((Order?)null);

        var result = await Sut.CancelOrder(new CancelOrderCommand(Guid.Parse("4D93A32F-9DFC-424A-A23B-32DA66317F1F")), CancellationToken.None);
        
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DontCancelOrder_WhenUserIsNotACreatorOfOrder()
    {
        GetCurrentIdentitySetup.Returns(new RecognizedUser(Guid.Parse("D9E8236C-FFCE-4A63-9F45-BA770E4D265D"), Role.Customer));
        GetOrderByIdSetup.ReturnsAsync(new Order
        {
            UserId = Guid.Parse("1637361F-B58A-457F-8370-0B65B30E715B"),
            OrderItems = []
        });
        
        var result = await Sut.CancelOrder(new CancelOrderCommand(Guid.Parse("4D93A32F-9DFC-424A-A23B-32DA66317F1F")), CancellationToken.None);
        
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DontCancelOrder_WhenOrderStatusIsNotPending()
    {
        var userId = Guid.Parse("E861A307-1E93-4BAC-9D07-083A4B4053C1");
        GetCurrentIdentitySetup.Returns(new RecognizedUser(userId, Role.Customer));
        GetOrderByIdSetup.ReturnsAsync(new Order
        {
            UserId = userId,
            OrderItems = [],
            Status = OrderStatus.Sent
        });

        var result = await Sut.CancelOrder(new CancelOrderCommand(Guid.Parse("65AF87D3-6EEB-4333-858B-CF30F50CA49D")), 
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task SuccessfullyCancelOrder()
    {
        var userId = Guid.Parse("E861A307-1E93-4BAC-9D07-083A4B4053C1");
        GetCurrentIdentitySetup.Returns(new RecognizedUser(userId, Role.Customer));
        GetOrderByIdSetup.ReturnsAsync(new Order
        {
            UserId = userId,
            OrderItems = [],
            Status = OrderStatus.Pending
        });

        var result = await Sut.CancelOrder(new CancelOrderCommand(Guid.Parse("65AF87D3-6EEB-4333-858B-CF30F50CA49D")), 
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DontUpdateOrderStatus_WhenOrderNotFound()
    {
        GetOrderByIdSetup.ReturnsAsync((Order?)null);

        var result = await Sut.UpdateOrderStatus(
            new UpdateOrderStatusCommand(Guid.Parse("65AF87D3-6EEB-4333-858B-CF30F50CA49D"), OrderStatus.Completed), 
            CancellationToken.None);
        
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