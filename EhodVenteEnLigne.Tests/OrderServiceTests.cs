using Xunit;
using Moq;
using EhodBoutiqueEnLigne.Models.Services;
using EhodBoutiqueEnLigne.Models.Repositories;
using EhodBoutiqueEnLigne.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Order = EhodBoutiqueEnLigne.Models.Entities.Order;
using System;
using EhodBoutiqueEnLigne.Models;
using System.Linq;
using System.Reflection;

public class OrderServiceTests
{
    [Fact]
    public async Task GetOrder_ReturnsOrder_WhenOrderExists()
    {
        // Arrange
        var mockOrderRepository = new Mock<IOrderRepository>();
        var orderService = new OrderService(null, mockOrderRepository.Object, null);
        var expectedOrder = new Order { Id = 1, Name = "Test Order" };
        mockOrderRepository.Setup(repo => repo.GetOrder(1)).Returns(Task.FromResult(expectedOrder));

        // Act
        var result = await orderService.GetOrder(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedOrder.Id, result.Id);
        Assert.Equal(expectedOrder.Name, result.Name);
    }

    [Fact]
    public async Task GetOrder_ReturnsNull_WhenOrderDoesNotExist()
    {
        // Arrange
        var mockOrderRepository = new Mock<IOrderRepository>();
        var orderService = new OrderService(null, mockOrderRepository.Object, null);
        mockOrderRepository.Setup(repo => repo.GetOrder(1)).Returns(Task.FromResult<Order>(null));

        // Act
        var result = await orderService.GetOrder(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetOrders_ReturnsListOfOrders()
    {
        // Arrange
        var mockOrderRepository = new Mock<IOrderRepository>();
        var orderService = new OrderService(null, mockOrderRepository.Object, null);

        // Créer une liste de commandes simulée avec les identifiants attendus
        var expectedOrders = new List<Order>
    {
        new Order { Id = 1 },
        new Order { Id = 2 }
    };

        // Configurer le comportement simulé du dépôt de commandes pour retourner les commandes simulées
        mockOrderRepository.Setup(repo => repo.GetOrders()).Returns(Task.FromResult((IList<Order>)expectedOrders));

        // Act
        var result = await orderService.GetOrders();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedOrders.Count, result.Count);
    }


    [Fact]
    public async Task GetOrders_ReturnsEmptyList_WhenNoOrdersExist()
    {
        // Arrange
        var mockOrderRepository = new Mock<IOrderRepository>();
        var orderService = new OrderService(null, mockOrderRepository.Object, null);
        var expectedOrders = new List<Order>();

        // Configurer le comportement simulé du dépôt de commandes pour retourner une tâche contenant une liste vide d'ordres
        mockOrderRepository.Setup(repo => repo.GetOrders()).Returns(Task.FromResult((IList<Order>)expectedOrders));

        // Act
        var result = await orderService.GetOrders();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }


    [Fact]
    public void UpdateInventory_CallsUpdateProductQuantitiesAndClear()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var mockCart = new Mock<ICart>();
        var orderService = new OrderService(mockCart.Object, null, mockProductService.Object);

        // Utiliser la réflexion pour accéder à la méthode privée
        var methodInfo = typeof(OrderService).GetMethod("UpdateInventory", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        methodInfo.Invoke(orderService, null);

        // Assert
        mockProductService.Verify(service => service.UpdateProductQuantities(), Times.Once);
        mockCart.Verify(cart => cart.Clear(), Times.Once);
    }
    [Fact]
    public void UpdateInventory_ThrowsException_WhenUpdateFails()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var mockCart = new Mock<ICart>();
        var orderService = new OrderService(mockCart.Object, null, mockProductService.Object);

        // Utiliser la réflexion pour accéder à la méthode privée
        var methodInfo = typeof(OrderService).GetMethod("UpdateInventory", BindingFlags.NonPublic | BindingFlags.Instance);

        // Configurer le comportement simulé pour lancer une exception lors de l'appel à la méthode UpdateInventory
        mockProductService.Setup(service => service.UpdateProductQuantities()).Throws<Exception>();

        // Act & Assert
        Assert.Throws<TargetInvocationException>(() => methodInfo.Invoke(orderService, null));
        // Assurez-vous que la méthode UpdateProductQuantities a été appelée exactement une fois
        mockProductService.Verify(service => service.UpdateProductQuantities(), Times.Once);
        // Assurez-vous que la méthode Clear n'a pas été appelée
        mockCart.Verify(cart => cart.Clear(), Times.Never);
    }


}
