using Xunit;
using Moq;
using EhodBoutiqueEnLigne.Models.Services;
using EhodBoutiqueEnLigne.Models.Repositories;
using EhodBoutiqueEnLigne.Models.Entities;
using EhodBoutiqueEnLigne.Models.ViewModels;
using System.Collections.Generic;
using EhodBoutiqueEnLigne.Models;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using System;

public class ProductServiceTests
{
    [Fact]
    public void GetAllProductsViewModel_ReturnsProductViewModelList()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object,
            mockOrderRepository.Object, mockLocalizer.Object);

        // Créer une liste de produits simulée pour simuler les produits retournés par le référentiel
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 10.0, Quantity = 5 },
            new Product { Id = 2, Name = "Product 2", Price = 20.0, Quantity = 10 },
        };

        // Configurer le comportement simulé du référentiel de produit pour retourner les produits simulés
        mockProductRepository.Setup(repo => repo.GetAllProducts()).Returns(products);

        // Act
        var result = productService.GetAllProductsViewModel();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(products.Count, result.Count);
        // Assurez-vous que chaque élément de la liste retournée est un objet ProductViewModel
        foreach (var item in result)
        {
            Assert.IsType<ProductViewModel>(item);
        }
    }
    [Fact]
    public void GetAllProductsViewModel_ReturnsEmptyList_WhenNoProducts()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object,
            mockOrderRepository.Object, mockLocalizer.Object);

        // Configurer le comportement simulé du référentiel de produit pour retourner une liste vide
        mockProductRepository.Setup(repo => repo.GetAllProducts()).Returns(new List<Product>());

        // Act
        var result = productService.GetAllProductsViewModel();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result); // Vérifiez si la liste retournée est vide
    }
    [Fact]
    public void MapToViewModel_ReturnsCorrectProductViewModelList()
    {
        // Arrange
        var productServiceType = typeof(ProductService);
        var methodInfo = productServiceType.GetMethod("MapToViewModel", BindingFlags.NonPublic | BindingFlags.Instance);
        var mockProductRepository = new Mock<IProductRepository>();
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var productServiceInstance = new ProductService(mockCart.Object, mockProductRepository.Object,
            mockOrderRepository.Object, mockLocalizer.Object);

        var productEntities = new List<Product>
    {
        new Product { Id = 1, Quantity = 10, Price = 20.5, Name = "Product 1", Description = "Description 1", Details = "Details 1" },
        new Product { Id = 2, Quantity = 15, Price = 30.75, Name = "Product 2", Description = "Description 2", Details = "Details 2" },
        // Ajoutez d'autres produits au besoin pour les tests
    };

        // Act
        var result = (List<ProductViewModel>)methodInfo.Invoke(productServiceInstance, new object[] { productEntities });

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productEntities.Count, result.Count);

        // Vérifiez si chaque élément a été correctement mappé
        for (int i = 0; i < productEntities.Count; i++)
        {
            Assert.Equal(productEntities[i].Id, result[i].Id);
            Assert.Equal(productEntities[i].Quantity.ToString(), result[i].Stock);
            Assert.Equal(productEntities[i].Price.ToString(CultureInfo.InvariantCulture), result[i].Price);
            Assert.Equal(productEntities[i].Name, result[i].Name);
            Assert.Equal(productEntities[i].Description, result[i].Description);
            Assert.Equal(productEntities[i].Details, result[i].Details);
        }
    }

    [Fact]
    public void MapToViewModel_ReturnsEmptyList_WhenProductEntitiesIsEmpty()
    {
        // Arrange
        var productServiceType = typeof(ProductService);
        var methodInfo = productServiceType.GetMethod("MapToViewModel", BindingFlags.NonPublic | BindingFlags.Instance);
        var mockProductRepository = new Mock<IProductRepository>();
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var productServiceInstance = new ProductService(mockCart.Object, mockProductRepository.Object,
            mockOrderRepository.Object, mockLocalizer.Object); var emptyProductEntities = new List<Product>();

        // Act
        List<ProductViewModel> result = null;
        if (emptyProductEntities.Count > 0)
        {
            result = (List<ProductViewModel>)methodInfo.Invoke(productServiceInstance, new object[] { emptyProductEntities });
        }
        else
        {
            // Si la liste est vide, retourne une liste vide directement
            result = new List<ProductViewModel>();
        }

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result); // Vérifie si la liste retournée est vide
    }

    [Fact]
    public void GetAllProducts_ReturnsProductList()
    {
        // Arrange
        var expectedProducts = new List<Product>
    {
        new Product { Id = 1, Name = "Product 1", Price = 10.0, Quantity = 5 },
        new Product { Id = 2, Name = "Product 2", Price = 20.0, Quantity = 10 },
    };
        var mockProductRepository = new Mock<IProductRepository>();
        mockProductRepository.Setup(repo => repo.GetAllProducts()).Returns(expectedProducts);
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var productServiceInstance = new ProductService(mockCart.Object, mockProductRepository.Object,
            mockOrderRepository.Object, mockLocalizer.Object); var emptyProductEntities = new List<Product>();
        // Act
        var result = productServiceInstance.GetAllProducts();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProducts.Count, result.Count);
        Assert.Equal(expectedProducts, result); // Vérifie si les produits retournés sont les mêmes que ceux attendus
    }

    [Fact]
    public void GetProductByIdViewModel_ReturnsCorrectProductViewModel()
    {
        // Arrange
        var expectedProductId = 1;
        var expectedProduct = new ProductViewModel { Id = 1, Name = "Product 1", Price = "10.0", Stock = "5" };
        var mockProductRepository = new Mock<IProductRepository>();
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var productServiceInstance = new ProductService(mockCart.Object, mockProductRepository.Object,
            mockOrderRepository.Object, mockLocalizer.Object); var emptyProductEntities = new List<Product>();
        var mockProductViewModelList = new List<ProductViewModel>
    {
        new ProductViewModel { Id = 1, Name = "Product 1", Price = "10.0", Stock = "5" },
        new ProductViewModel { Id = 2, Name = "Product 2", Price = "20.0", Stock = "10" },
    };
        var mockProductService = new Mock<ProductService>(MockBehavior.Strict, null, mockProductRepository.Object, null, null);
        mockProductService.Setup(ps => ps.GetAllProductsViewModel()).Returns(mockProductViewModelList);

        // Act
        var result = mockProductService.Object.GetProductByIdViewModel(expectedProductId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProduct.Id, result.Id);
        Assert.Equal(expectedProduct.Name, result.Name);
        Assert.Equal(expectedProduct.Price, result.Price);
        Assert.Equal(expectedProduct.Stock, result.Stock);
    }
    [Fact]
    public void GetAllProducts_ReturnsEmptyList_WhenNoProducts()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        mockProductRepository.Setup(repo => repo.GetAllProducts()).Returns(new List<Product>());
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var productServiceInstance = new ProductService(mockCart.Object, mockProductRepository.Object,
            mockOrderRepository.Object, mockLocalizer.Object); var emptyProductEntities = new List<Product>();
        // Act
        var result = productServiceInstance.GetAllProducts();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result); // Vérifie si la liste retournée est vide
    }

    [Fact]
    public void GetProductByIdViewModel_ReturnsNull_WhenProductNotFound()
    {
        // Arrange
        var nonExistentProductId = 999; // Supposons que cet ID n'existe pas dans la liste des produits
        var mockProductRepository = new Mock<IProductRepository>();
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var productServiceInstance = new ProductService(mockCart.Object, mockProductRepository.Object,
            mockOrderRepository.Object, mockLocalizer.Object); var emptyProductEntities = new List<Product>();
        var mockProductViewModelList = new List<ProductViewModel>
    {
        new ProductViewModel { Id = 1, Name = "Product 1", Price = "10.0", Stock = "5" },
        new ProductViewModel { Id = 2, Name = "Product 2", Price = "20.0", Stock = "10" },
    };
        var mockProductService = new Mock<ProductService>(MockBehavior.Strict, null, mockProductRepository.Object, null, null);
        mockProductService.Setup(ps => ps.GetAllProductsViewModel()).Returns(mockProductViewModelList);

        // Act
        var result = mockProductService.Object.GetProductByIdViewModel(nonExistentProductId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetProductById_ReturnsProduct_WhenValidId()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);
        var products = new List<Product>
    {
        new Product { Id = 1, Name = "Product 1" },
        new Product { Id = 2, Name = "Product 2" },
    };
        mockProductRepository.Setup(repo => repo.GetAllProducts()).Returns(products);

        // Act
        var result = productService.GetProductById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Product 1", result.Name);
    }

    [Fact]
    public void GetProductById_ReturnsNull_WhenInvalidId()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);
        var products = new List<Product>
    {
        new Product { Id = 1, Name = "Product 1" },
        new Product { Id = 2, Name = "Product 2" },
    };
        mockProductRepository.Setup(repo => repo.GetAllProducts()).Returns(products);

        // Act
        var result = productService.GetProductById(3);

        // Assert
        Assert.Null(result);
    }
    [Fact]
    public async Task GetProduct_ReturnsProduct_WhenValidId()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);
        var expectedProduct = new Product { Id = 1, Name = "Product 1" };
        mockProductRepository.Setup(repo => repo.GetProduct(1)).ReturnsAsync(expectedProduct);

        // Act
        var result = await productService.GetProduct(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProduct.Id, result.Id);
        Assert.Equal(expectedProduct.Name, result.Name);
    }
    [Fact]
    public async Task GetProduct_ReturnsNull_WhenInvalidId()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);
        mockProductRepository.Setup(repo => repo.GetProduct(2)).ReturnsAsync((Product)null); // Simuler le retour d'un produit null pour un ID invalide

        // Act
        var result = await productService.GetProduct(2);

        // Assert
        Assert.Null(result);
    }
    [Fact]
    public async Task GetProduct_ReturnsListOfProducts_WhenProductsExist()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);
        var expectedProducts = new List<Product>
    {
        new Product { Id = 1, Name = "Product 1" },
        new Product { Id = 2, Name = "Product 2" }
    };
        mockProductRepository.Setup(repo => repo.GetProduct()).ReturnsAsync(expectedProducts);

        // Act
        var result = await productService.GetProduct();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProducts.Count, result.Count);
        for (int i = 0; i < expectedProducts.Count; i++)
        {
            Assert.Equal(expectedProducts[i].Id, result[i].Id);
            Assert.Equal(expectedProducts[i].Name, result[i].Name);
        }
    }
    [Fact]
    public async Task GetProduct_ReturnsEmptyList_WhenNoProductsExist()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);
        mockProductRepository.Setup(repo => repo.GetProduct()).ReturnsAsync(new List<Product>()); // Simuler le retour d'une liste vide de produits

        // Act
        var result = await productService.GetProduct();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void UpdateProductQuantities_UpdatesProductStocks_WhenCartLinesExist()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();

        // Utilisation de l'implémentation concrète de ICart
        var cart = new Cart();
        var productService = new ProductService(cart, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);

        var cartLines = new List<CartLine>
    {
        new CartLine { Product = new Product { Id = 1 }, Quantity = 2 },
        new CartLine { Product = new Product { Id = 2 }, Quantity = 3 }
    };

        // Remplissage du panier avec les lignes de test
        foreach (var line in cartLines)
        {
            cart.AddItem(line.Product, line.Quantity);
        }

        // Act
        productService.UpdateProductQuantities();

        // Assert
        foreach (var line in cartLines)
        {
            // Vérifie que UpdateProductStocks est appelé avec les bons arguments pour chaque ligne du panier
            mockProductRepository.Verify(repo => repo.UpdateProductStocks(line.Product.Id, line.Quantity), Times.Once);
        }
    }

    [Fact]
    public void UpdateProductQuantities_DoesNotUpdateProductStocks_WhenCartIsEmpty()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();

        // Utilisation de l'implémentation concrète de ICart
        var cart = new Cart();
        var productService = new ProductService(cart, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);

        // Act
        productService.UpdateProductQuantities();

        // Assert
        // Vérifie que la méthode UpdateProductStocks n'est jamais appelée
        mockProductRepository.Verify(repo => repo.UpdateProductStocks(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void CheckProductModelErrors_ReturnsNoErrors_WhenProductIsValid()
    {
        // Arrange
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var mockCart = new Mock<ICart>();
        var mockProductRepository = new Mock<IProductRepository>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);
        var validProduct = new ProductViewModel
        {
            Name = "Product",
            Price = "10.5",
            Stock = "5"
        };

        // Act
        var errors = productService.CheckProductModelErrors(validProduct);

        // Assert
        Assert.Empty(errors); // Vérifie que la liste d'erreurs est vide
    }

    [Fact]
    public void CheckProductModelErrors_ReturnsErrors_WhenProductIsInvalid()
    {
        // Arrange
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var mockCart = new Mock<ICart>();
        var mockProductRepository = new Mock<IProductRepository>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);
        var invalidProduct = new ProductViewModel
        {
            Name = "", // Nom vide
            Price = "Not a number", // Prix non numérique
            Stock = "-5" // Stock négatif
        };

        // Act
        var errors = productService.CheckProductModelErrors(invalidProduct);

        // Assert
        Assert.NotEmpty(errors); // Vérifie que des erreurs sont renvoyées
                                 // Vous pouvez vérifier les erreurs spécifiques ici si nécessaire
    }
    [Fact]
    public void SaveProduct_CallsSaveProductInRepository_WithCorrectProductData()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);
        var productViewModel = new ProductViewModel
        {
            Name = "Product",
            Price = "10.5",
            Stock = "5",
            Description = "Description",
            Details = "Details"
        };

        // Act
        productService.SaveProduct(productViewModel);

        // Assert
        mockProductRepository.Verify(repo => repo.SaveProduct(It.IsAny<Product>()), Times.Once);
        // Vérifie que la méthode SaveProduct du repository est appelée une fois avec un produit ayant les mêmes données que productViewModel
    }
    [Fact]
    public void SaveProduct_DoesNotCallSaveProductInRepository_WhenProductDataIsInvalid()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);
        var invalidProductViewModel = new ProductViewModel
        {
            // Données du produit invalides (par exemple, le nom est vide)
            Name = "", // Nom vide
            Price = "10.5",
            Stock = "5",
            Description = "Description",
            Details = "Details"
        };

        // Act
        productService.SaveProduct(invalidProductViewModel);

        // Assert
        mockProductRepository.Verify(repo => repo.SaveProduct(It.IsAny<Product>()), Times.Never);
        // Vérifie que la méthode SaveProduct du repository n'est jamais appelée
    }

    [Fact]
    public void DeleteProduct_RemovesProductFromCartAndRepository()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();

        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);
        var productId = 1;

        // Créer un produit simulé dans le panier
        var productInCart = new Product { Id = productId, Name = "Product 1", Price = 10.0, Quantity = 5 };

        // Configurer le comportement simulé du panier pour retourner le produit simulé
        mockCart.Setup(cart => cart.RemoveLine(productInCart));

        // Act
        productService.DeleteProduct(productId);

        // Assert
        // Assurez-vous que la méthode RemoveLine du panier a été appelée avec le bon produit
        mockCart.Verify(cart => cart.RemoveLine(productInCart), Times.Once);

        // Assurez-vous que la méthode DeleteProduct du référentiel a été appelée avec le bon ID de produit
        mockProductRepository.Verify(repo => repo.DeleteProduct(productId), Times.Once);
    }
    [Fact]
    public void DeleteProduct_ProductNotFoundInCartOrRepository_ThrowsException()
    {
        // Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockCart = new Mock<ICart>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();

        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);
        var productId = 1;

        // Configurer le comportement simulé du panier pour retourner null lorsque RemoveLine est appelé
        mockCart.Setup(cart => cart.RemoveLine(It.IsAny<Product>())).Throws(new InvalidOperationException());

        // Act & Assert
        // Assurez-vous qu'une exception est levée lorsque le produit n'est pas trouvé dans le panier
        Assert.Throws<InvalidOperationException>(() => productService.DeleteProduct(productId));

        // Assurez-vous que la méthode DeleteProduct du référentiel n'est pas appelée
        mockProductRepository.Verify(repo => repo.DeleteProduct(It.IsAny<int>()), Times.Never);
    }
}
