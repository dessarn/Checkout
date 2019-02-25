using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketBasket.Services.Interfaces;
using SuperMarketBasket.Services;
using SuperMarketBasket.DataObjects;
using Moq;
using SuperMarketBasket.DataStore.Interfaces;

namespace SuperMarketBasket.Test.Services
{
    [TestFixture]
    class ServiceTests
    {
        private IBasketService basketService;

        private Mock<IFakeDataStore> mockFakeDataStore;

        [SetUp]
        public void Setup()
        {
            mockFakeDataStore = new Mock<IFakeDataStore>();
            basketService = new BasketService(mockFakeDataStore.Object);
        }

        [Test]
        public void GetProducts_ReturnsListOfProducts()
        {
            // Arrange
            Type expected = typeof(List<Product>);
            mockFakeDataStore.Setup(m => m.FetchData()).Returns(new List<Product>());
            // Act
            var actual = basketService.GetProducts();
            // Assert
            Assert.AreEqual(expected, actual.GetType());
        }

        [Test]
        public void GetProducts_CallsDataStore()
        {
            // Arrange
            mockFakeDataStore.Setup(m => m.FetchData()).Returns(new List<Product>());
            // Act
            var actual = basketService.GetProducts();
            // Assert
            mockFakeDataStore.Verify(m => m.FetchData(), Times.Once);
        }

        [Test]
        public void CalculateBasketTotal_ReturnsDecimal()
        {
            // Arrange
            Type expected = typeof(Decimal);
            // Act
            var actual = basketService.CalculateBasketTotal(new List<Product>());
            // Assert
            Assert.AreEqual(expected, actual.GetType());
        }

        [Test]
        public void CalculateBasketTotal_ReturnTotalForBasicItems()
        {
            // Arrange
            var expected = 12.9m;
            var listOfItems = new List<Product>()
            {
                new Product("", "", 3.43m, null),
                new Product("", "", 2.54m, null),
                new Product("", "", 6.93m, null)
            };
            // Act
            var actual = basketService.CalculateBasketTotal(listOfItems);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateBasketTotal_ReturnTotalForItemsWithSpecialOffer()
        {
            // Arrange
            var expected = 3.0m;
            var listOfItems = new List<Product>()
            {
                new Product("F46", "", 2.00m, new SpecialOffer(3, 3.00m)),
                new Product("F46", "", 2.00m, new SpecialOffer(3, 3.00m)),
                new Product("F46", "", 2.00m, new SpecialOffer(3, 3.00m))
            };
            // Act
            var actual = basketService.CalculateBasketTotal(listOfItems);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateBasketTotal_ReturnTotalForMultipleItemsWithSpecialOffers()
        {
            // Arrange
            var expected = 5.0m;
            var listOfItems = new List<Product>()
            {
                new Product("F46", "", 2.00m, new SpecialOffer(3, 3.00m)),
                new Product("B34", "", 1.50m, new SpecialOffer(2, 2.00m)),
                new Product("F46", "", 2.00m, new SpecialOffer(3, 3.00m)),
                new Product("F46", "", 2.00m, new SpecialOffer(3, 3.00m)),
                new Product("B34", "", 1.50m, new SpecialOffer(2, 2.00m)),
            };
            // Act
            var actual = basketService.CalculateBasketTotal(listOfItems);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateBasketTotal_ReturnTotalForSameItemsWithTheSameSpecialOffer()
        {
            // Arrange
            var expected = 6.0m;
            var listOfItems = new List<Product>()
            {
                new Product("B34", "", 1.50m, new SpecialOffer(2, 2.00m)),
                new Product("B34", "", 1.50m, new SpecialOffer(2, 2.00m)),
                new Product("B34", "", 1.50m, new SpecialOffer(2, 2.00m)),
                new Product("B34", "", 1.50m, new SpecialOffer(2, 2.00m)),
                new Product("B34", "", 1.50m, new SpecialOffer(2, 2.00m)),
                new Product("B34", "", 1.50m, new SpecialOffer(2, 2.00m)),
            };
            // Act
            var actual = basketService.CalculateBasketTotal(listOfItems);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateBasketTotal_ReturnTotalForSameItemsWithIncompleteSpecialOffer()
        {
            // Arrange
            var expected = 5.5m;
            var listOfItems = new List<Product>()
            {
                new Product("B34", "", 1.50m, new SpecialOffer(2, 2.00m)),
                new Product("B34", "", 1.50m, new SpecialOffer(2, 2.00m)),
                new Product("B34", "", 1.50m, new SpecialOffer(2, 2.00m)),
                new Product("B34", "", 1.50m, new SpecialOffer(2, 2.00m)),
                new Product("B34", "", 1.50m, new SpecialOffer(2, 2.00m)),
            };
            // Act
            var actual = basketService.CalculateBasketTotal(listOfItems);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateBasketTotal_ReturnTotalForOneItemsWithIncompleteSpecialOffer()
        {
            // Arrange
            var expected = 2.0m;
            var listOfItems = new List<Product>()
            {
                new Product("F46", "", 2.00m, new SpecialOffer(3, 3.00m))
            };
            // Act
            var actual = basketService.CalculateBasketTotal(listOfItems);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ChangeSpecialOffer_returnsBool()
        {
            // Arrange
            Type expected = typeof(bool);
            // Act
            var actual = basketService.ChangeSpecialOffer(null, null);
            // Assert
            Assert.AreEqual(expected, actual.GetType());
        }

        [Test]
        public void ChangeSpecialOffer_returnsFalseWhenProductIsNotFound()
        {
            // Arrange
            bool expected = false;
            mockFakeDataStore.Setup(m => m.FetchItem(null)).Returns<Product>(null);
            // Act
            var actual = basketService.ChangeSpecialOffer(null, null);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ChangeSpecialOffer_returnsTrueWhenConditionsAreMet()
        {
            // Arrange
            bool expected = true;
            var mockProduct = new Product(null, null, 6m, null);
            var mockOffer = new SpecialOffer(2, 4.00m);
            mockFakeDataStore.Setup(m => m.FetchItem("Something")).Returns(mockProduct);
            mockFakeDataStore.Setup(m => m.UpdateItem(mockProduct)).Returns(true);
            // Act
            var actual = basketService.ChangeSpecialOffer("Something", mockOffer);
            // Assert
            Assert.AreEqual(expected, actual);
        }

    }
}
