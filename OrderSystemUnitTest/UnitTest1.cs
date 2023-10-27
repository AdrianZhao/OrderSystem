using Moq;
using System.Data;
using OrderSystem.Data;
using OrderSystem.Models;
using OrderSystem.Models.BusinessLogicLayer;
using Castle.Components.DictionaryAdapter;
using OrderSystem.Models.ViewModel;

namespace OrderSystemUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private Mock<IRepository<Product>> _productRepo = new Mock<IRepository<Product>>();
        private Mock<IRepository<InCartProduct>> _inCartProductRepo = new Mock<IRepository<InCartProduct>>();
        private Mock<IRepository<Cart>> _cartRepo = new Mock<IRepository<Cart>>();
        private Mock<IRepository<InOrderProduct>> _inOrderProductRepo = new Mock<IRepository<InOrderProduct>>();
        private Mock<IRepository<Order>> _orderRepo = new Mock<IRepository<Order>>();
        private Mock<IRepository<Country>> _countryRepo = new Mock<IRepository<Country>>();
        public OrderSystemBusinessLogicLayer InitializeBLL()
        {
            return new OrderSystemBusinessLogicLayer(_productRepo.Object, _inCartProductRepo.Object, _cartRepo.Object, _orderRepo.Object,_countryRepo.Object, _inOrderProductRepo.Object);
        }
        [TestMethod]
        public void GetProductByValidId_ProductExists_ShouldReturnProduct()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();           
            Guid productId = Guid.NewGuid();
            Product product = new Product { Id = productId };
            _productRepo.Setup(p => p.Get(productId)).Returns(product);

            // Act
            Product result = orderSystemBusinessLogicLayer.GetProductById(productId);

            // Assert
            _productRepo.Verify(r => r.Get(productId));
            Assert.AreEqual(product, result);
        }
        [TestMethod]
        public void GetProductByInvalidId_ProductNotExists_ThrowsInvalidOperation()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();
            Guid invalidProductId = Guid.NewGuid();
            Guid guid = Guid.NewGuid();
            Product product = new Product { Id = guid };
            _productRepo.Setup(p => p.Get(guid)).Returns(product);

            // Act And Assert
            Assert.ThrowsException<InvalidOperationException>(() => orderSystemBusinessLogicLayer.GetProductById(invalidProductId));
        }
        [TestMethod]
        public void GetCart_CartExists_ShouldReturnCart()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();
            Cart cart = new Cart();
            _cartRepo.Setup(c => c.GetAll()).Returns(new List<Cart> { cart });

            // Act
            Cart result = orderSystemBusinessLogicLayer.GetCart();

            // Assert
            _cartRepo.Verify(r => r.GetAll());
            Assert.AreEqual(cart, result);
        }
        [TestMethod]
        public void GetCart_CartDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();
            _cartRepo.Setup(c => c.GetAll()).Returns(new List<Cart>());

            // Act
            Cart result = orderSystemBusinessLogicLayer.GetCart();

            // Assert
            _cartRepo.Verify(r => r.GetAll());
            Assert.IsNull(result);
        }
        [TestMethod]
        public void ListProducts_ShouldReturnOrderedProducts()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();
            List<Product> products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product C" },
                new Product { Id = Guid.NewGuid(), Name = "Product A" },
                new Product { Id = Guid.NewGuid(), Name = "Product B" }
            };
            _productRepo.Setup(p => p.GetAll()).Returns(products);

            // Act
            List<Product> result = orderSystemBusinessLogicLayer.ListProducts().ToList();

            // Assert
            _productRepo.Verify(r => r.GetAll());
            CollectionAssert.AreEqual(products.OrderBy(p => p.Name).ToList(), result.ToList());
        }
        [TestMethod]
        public void SearchProducts_ShouldReturnFilteredAndOrderedProducts()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();
            List<Product> products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Apple", Description = "A delicious fruit" },
                new Product { Id = Guid.NewGuid(), Name = "Banana", Description = "A delicious one" },
                new Product { Id = Guid.NewGuid(), Name = "Orange", Description = "A citrus fruit" }
            };
            _productRepo.Setup(p => p.GetAll()).Returns(products);

            // Act
            List<Product> result = orderSystemBusinessLogicLayer.SearchProducts("fruit").ToList();

            // Assert
            _productRepo.Verify(r => r.GetAll());
            List<Product> expected = products
                .Where(p => p.Name.Contains("fruit") || p.Description.Contains("fruit"))
                .OrderBy(p => p.Name)
                .ToList();

            CollectionAssert.AreEqual(expected, result);
        }
        [TestMethod]
        public void CreateCart_ShouldReturnEmptyCart()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();

            // Act
            Cart result = orderSystemBusinessLogicLayer.CreateCart();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Products.Count);
        }
        [TestMethod]
        public void AddProductToCart_CartDoesNotExist_ShouldCreateCartAndAddProduct()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();
            Guid productId = Guid.NewGuid();
            int initialQuantity = 5;
            Product product = new Product { Id = productId, AvailableQuantity = initialQuantity };
            Cart cart = null; 
            _productRepo.Setup(p => p.Get(productId)).Returns(product);
            _cartRepo.Setup(c => c.GetAll()).Returns(new List<Cart>()); 
            _cartRepo.Setup(r => r.Create(It.IsAny<Cart>()));
            _inCartProductRepo.Setup(r => r.Create(It.IsAny<InCartProduct>()));

            // Act
            Cart result = orderSystemBusinessLogicLayer.AddProductToCart(productId);

            // Assert
            _productRepo.Verify(r => r.Get(productId));
            _cartRepo.Verify(r => r.GetAll());
            _inCartProductRepo.Verify(r => r.Create(It.IsAny<InCartProduct>()));
            Assert.AreEqual(1, result.Products.Count);
            Assert.AreEqual(1, result.Products.First().Quantity);
            Assert.AreEqual(initialQuantity - 1, result.Products.First().Product.AvailableQuantity);
        }
        [TestMethod]
        public void AddProductToCart_ProductExistsInCart_ShouldIncrementQuantity()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();
            Guid productId = Guid.NewGuid();
            int initialQuantity = 5;
            int initialCartQuantity = 3;
            Product product = new Product { Id = productId, AvailableQuantity = initialQuantity };
            Cart cart = new Cart
            {
                Products = new HashSet<InCartProduct>
                {
                    new InCartProduct { Product = product, Quantity = initialCartQuantity }
                }
            };
            _productRepo.Setup(p => p.Get(productId)).Returns(product);
            _cartRepo.Setup(c => c.GetAll()).Returns(new List<Cart> { cart });

            // Act
            Cart result = orderSystemBusinessLogicLayer.AddProductToCart(productId);

            // Assert
            _productRepo.Verify(r => r.Get(productId));
            _cartRepo.Verify(r => r.GetAll());
            _inCartProductRepo.Verify(r => r.Update(It.IsAny<InCartProduct>()));
            Assert.AreEqual(initialCartQuantity + 1, result.Products.First().Quantity);
            Assert.AreEqual(initialQuantity - 1, result.Products.First().Product.AvailableQuantity);
        }
        [TestMethod]
        public void AddProductToCart_ProductNotExistsInCart_ShouldAddToCart()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();
            Guid productId = Guid.NewGuid();
            int initialQuantity = 5;
            Product product = new Product { Id = productId, AvailableQuantity = initialQuantity };
            Cart cart = null;
            _productRepo.Setup(p => p.Get(productId)).Returns(product);
            _cartRepo.Setup(c => c.GetAll()).Returns(new List<Cart>());
            _inCartProductRepo.Setup(r => r.Create(It.IsAny<InCartProduct>()));

            // Act
            Cart result = orderSystemBusinessLogicLayer.AddProductToCart(productId);

            // Assert
            _productRepo.Verify(r => r.Get(productId));
            _cartRepo.Verify(r => r.GetAll());
            _inCartProductRepo.Verify(r => r.Create(It.IsAny<InCartProduct>()));
            Assert.AreEqual(1, result.Products.Count);
            Assert.AreEqual(1, result.Products.First().Quantity);
            Assert.AreEqual(initialQuantity - 1, result.Products.First().Product.AvailableQuantity);
        }
        [TestMethod]
        public void ReduceProduct_QuantityGreaterThanOne_ShouldReduceQuantity()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();
            Guid productId = Guid.NewGuid();
            int initialQuantity = 5;
            int initialCartQuantity = 3;
            Product product = new Product { Id = productId, AvailableQuantity = initialQuantity };
            Cart cart = new Cart
            {
                Products = new HashSet<InCartProduct>
                {
                    new InCartProduct { Product = product, Quantity = initialCartQuantity }
                }
            };
            _productRepo.Setup(p => p.Get(productId)).Returns(product);
            _cartRepo.Setup(c => c.GetAll()).Returns(new List<Cart> { cart });
            _cartRepo.Setup(r => r.Update(cart));

            // Act
            Cart result = orderSystemBusinessLogicLayer.ReduceProduct(productId);

            // Assert
            _productRepo.Verify(r => r.Get(productId));
            _cartRepo.Verify(r => r.GetAll());
            _cartRepo.Verify(r => r.Update(cart));
            Assert.AreEqual(initialCartQuantity - 1, result.Products.First().Quantity);
            Assert.AreEqual(initialQuantity + 1, result.Products.First().Product.AvailableQuantity);
        }

        [TestMethod]
        public void ReduceProduct_QuantityEqualsOne_ShouldRemoveProductFromCart()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();
            Guid productId = Guid.NewGuid();
            int initialQuantity = 5;
            Product product = new Product { Id = productId, AvailableQuantity = initialQuantity };
            Cart cart = new Cart
            {
                Products = new HashSet<InCartProduct>
                {
                    new InCartProduct { Product = product, Quantity = 1 }
                }
            };
            _productRepo.Setup(p => p.Get(productId)).Returns(product);
            _cartRepo.Setup(c => c.GetAll()).Returns(new List<Cart> { cart }); 
            _cartRepo.Setup(r => r.Update(cart)); 
            _inCartProductRepo.Setup(r => r.Delete(It.IsAny<InCartProduct>())); 

            // Act
            Cart result = orderSystemBusinessLogicLayer.ReduceProduct(productId);

            // Assert
            _productRepo.Verify(r => r.Get(productId));
            _cartRepo.Verify(r => r.GetAll());
            _cartRepo.Verify(r => r.Update(cart));
            _inCartProductRepo.Verify(r => r.Delete(It.IsAny<InCartProduct>()));
            Assert.AreEqual(0, result.Products.Count);
            Assert.AreEqual(initialQuantity + 1, product.AvailableQuantity);
        }
        [TestMethod]
        public void RemoveProduct_ShouldRemoveProductFromCart()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();
            Guid productId = Guid.NewGuid();
            int initialQuantity = 5;
            int initialCartQuantity = 3; 
            Product product = new Product { Id = productId, AvailableQuantity = initialQuantity };
            Cart cart = new Cart
            {
                Products = new HashSet<InCartProduct>
                {
                    new InCartProduct { Product = product, Quantity = initialCartQuantity }
                }
            };
            _productRepo.Setup(p => p.Get(productId)).Returns(product);
            _cartRepo.Setup(c => c.GetAll()).Returns(new List<Cart> { cart }); 
            _cartRepo.Setup(r => r.Update(cart)); 
            _inCartProductRepo.Setup(r => r.Delete(It.IsAny<InCartProduct>())); 

            // Act
            Cart result = orderSystemBusinessLogicLayer.RemoveProduct(productId);

            // Assert
            _productRepo.Verify(r => r.Get(productId));
            _cartRepo.Verify(r => r.GetAll());
            _cartRepo.Verify(r => r.Update(cart));
            _inCartProductRepo.Verify(r => r.Delete(It.IsAny<InCartProduct>()));
            Assert.AreEqual(0, result.Products.Count);
            Assert.AreEqual(initialQuantity + initialCartQuantity, product.AvailableQuantity);
        }
        [TestMethod]
        public void EmptyCart_ShouldClearCartAndRestoreProductQuantities()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();
            Guid productId1 = Guid.NewGuid();
            Guid productId2 = Guid.NewGuid();
            int initialQuantity1 = 5; 
            int initialQuantity2 = 7; 
            int cartQuantity1 = 3;
            int cartQuantity2 = 2;
            Product product1 = new Product { Id = productId1, AvailableQuantity = initialQuantity1 };
            Product product2 = new Product { Id = productId2, AvailableQuantity = initialQuantity2 };
            Cart cart = new Cart
            {
                Products = new HashSet<InCartProduct>
                {
                    new InCartProduct { Product = product1, Quantity = cartQuantity1 },
                    new InCartProduct { Product = product2, Quantity = cartQuantity2 }
                }
            };
            _productRepo.Setup(p => p.Get(productId1)).Returns(product1);
            _productRepo.Setup(p => p.Get(productId2)).Returns(product2);
            _cartRepo.Setup(c => c.GetAll()).Returns(new List<Cart> { cart }); 
            _inCartProductRepo.Setup(r => r.GetAll()).Returns(cart.Products.ToList()); 
            _cartRepo.Setup(r => r.Update(cart)); 
            _inCartProductRepo.Setup(r => r.Delete(It.IsAny<InCartProduct>())); 

            // Act
            Cart result = orderSystemBusinessLogicLayer.EmptyCart();

            // Assert
            _cartRepo.Verify(r => r.GetAll());
            _inCartProductRepo.Verify(r => r.GetAll());
            _productRepo.Verify(r => r.Get(productId1));
            _productRepo.Verify(r => r.Get(productId2));
            _cartRepo.Verify(r => r.Update(cart));
            _inCartProductRepo.Verify(r => r.Delete(It.IsAny<InCartProduct>()));
            Assert.AreEqual(0, result.Products.Count);
            Assert.AreEqual(initialQuantity1 + cartQuantity1, product1.AvailableQuantity);
            Assert.AreEqual(initialQuantity2 + cartQuantity2, product2.AvailableQuantity);
        }
        [TestMethod]
        public void GetCountries_ShouldReturnListOfCountries()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();
            List<Country> countriesList = new List<Country>
            {
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "Canada",
                    ConversionRate = 1.0m
                },
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "USA",
                    ConversionRate = 0.73m
                },
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "Mexico",
                    ConversionRate = 13.29m
                }
            };
            _countryRepo.Setup(c => c.GetAll()).Returns(countriesList);

            // Act
            List<Country> result = orderSystemBusinessLogicLayer.GetCountries().ToList();

            // Assert
            _countryRepo.Verify(c => c.GetAll());
            CollectionAssert.AreEqual(countriesList, result);
        }
        [TestMethod]
        public void GetSelectedCountry_ShouldReturnSelectedCountry()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();
            List<Country> countriesList = new List<Country>
            {
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "Canada",
                    ConversionRate = 1.0m
                },
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "USA",
                    ConversionRate = 0.73m
                },
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "Mexico",
                    ConversionRate = 13.29m
                }
            };
            _countryRepo.Setup(c => c.GetAll()).Returns(countriesList);

            // Act
            string selectedCountryName = countriesList.First().Id.ToString();
            Country result = orderSystemBusinessLogicLayer.GetSelectedCountry(selectedCountryName);

            // Assert
            _countryRepo.Verify(c => c.GetAll());
            Assert.AreEqual(selectedCountryName, result.Id.ToString());
        }
        [TestMethod]
        public void ConfirmOrder_ShouldReturnConfirmOrderViewModel()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();
            List<Country> countriesList = new List<Country>
            {
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "Canada",
                    ConversionRate = 1.0m
                },
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "USA",
                    ConversionRate = 0.73m
                },
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "Mexico",
                    ConversionRate = 13.29m
                }
            };
            Cart cart = new Cart
            {
                Products = new HashSet<InCartProduct>
                {
                    new InCartProduct
                    {
                        Product = new Product
                        {
                            Id = Guid.NewGuid(),
                            Name = "Product1",
                            Price = 10.0m
                        },
                        Quantity = 2
                    },
                    new InCartProduct
                    {
                        Product = new Product
                        {
                            Id = Guid.NewGuid(),
                            Name = "Product2",
                            Price = 15.0m
                        },
                        Quantity = 3
                    }
                }
            };
            string selectedCountryName = countriesList.First().Id.ToString();
            _countryRepo.Setup(c => c.GetAll()).Returns(countriesList);
            _cartRepo.Setup(c => c.GetAll()).Returns(new List<Cart> { cart });

            // Act
            ConfirmOrderViewModel result = orderSystemBusinessLogicLayer.ConfirmOrder(selectedCountryName);

            // Assert
            _countryRepo.Verify(c => c.GetAll());
            _cartRepo.Verify(c => c.GetAll());
            Assert.IsNotNull(result);
            Assert.AreEqual(cart, result.Cart);
            Assert.AreEqual(countriesList[0], result.Country);
        }
        [TestMethod]
        public void MakeOrder_ShouldReturnOrder()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();
            List<Country> countriesList = new List<Country>
            {
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "Canada",
                    ConversionRate = 1.0m
                }
            };
            Cart cart = new Cart
            {
                Products = new HashSet<InCartProduct>
                {
                    new InCartProduct
                    {
                        Product = new Product
                        {
                            Id = Guid.NewGuid(),
                            Name = "Product1",
                            Price = 10.0m,
                            AvailableQuantity = 5 // Add available quantity
                        },
                        Quantity = 2
                    }
                }
            };
            ConfirmOrderViewModel confirmOrderViewModel = new ConfirmOrderViewModel
            {
                Cart = cart,
                CountryId = countriesList[0].Id,
                Country = countriesList[0],
                Address = "123 Main St",
                MailingCode = "12345",
                TotalPrice = 23.46m,
                ConvertedPrice = 20.0m
            };
            _countryRepo.Setup(c => c.GetAll()).Returns(countriesList);
            _cartRepo.Setup(c => c.GetAll()).Returns(new List<Cart> { cart });

            // Act
            orderSystemBusinessLogicLayer.MakeOrder(confirmOrderViewModel);

            // Assert
            _orderRepo.Verify(o => o.Create(It.IsAny<Order>()));
        }
        [TestMethod]
        public void ListOrders_ShouldReturnOrders()
        {
            // Arrange
            OrderSystemBusinessLogicLayer orderSystemBusinessLogicLayer = InitializeBLL();

            // Create a list of sample orders
            List<Order> orders = new List<Order>
            {
                new Order
                {
                    Id = Guid.NewGuid(),
                    Products = new HashSet<InOrderProduct>
                    {
                        new InOrderProduct
                        {
                            Product = new Product
                            {
                                Id = Guid.NewGuid(),
                                Name = "Product1",
                                Price = 10.0m
                            },
                            Quantity = 2
                        }
                    },
                    DestinationCountry = "Canada",
                    Address = "123 Main St",
                    MailingCode = "12345",
                    TotalPrice = 23.46m
                },
                new Order
                {
                    Id = Guid.NewGuid(),
                    Products = new HashSet<InOrderProduct>
                    {
                        new InOrderProduct
                        {
                            Product = new Product
                            {
                                Id = Guid.NewGuid(),
                                Name = "Product2",
                                Price = 15.0m
                            },
                            Quantity = 3
                        }
                    },
                    DestinationCountry = "USA",
                    Address = "456 Elm St",
                    MailingCode = "67890",
                    TotalPrice = 40.75m
                }
            };
            _orderRepo.Setup(o => o.GetAll()).Returns(orders);

            // Act
            List<Order> result = orderSystemBusinessLogicLayer.ListOrders().ToList();

            // Assert
            _orderRepo.Verify(o => o.GetAll());
            CollectionAssert.AreEqual(orders, result);
        }
    }
}