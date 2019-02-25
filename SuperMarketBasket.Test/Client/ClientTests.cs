using Moq;
using NUnit.Framework;
using SuperMarketBasket.Client;
using SuperMarketBasket.Client.Interfaces;
using SuperMarketBasket.DataObjects;
using SuperMarketBasket.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace SuperMarketBasket.Test.Client
{
    [TestFixture]
    class ClientTests
    {
        private IBasketClient client;

        private Mock<IBasketService> mockBasketService;

        [SetUp]
        public void Setup()
        {
            mockBasketService = new Mock<IBasketService>();
            client = new BasketClient(mockBasketService.Object);
        }

        [Test]
        public void Checkout_ReturnsDecimal()
        {
            // Arrange
            Type expected = typeof(Decimal);
            // Act
            var actual = client.Checkout();
            // Assert
            Assert.AreEqual(expected, actual.GetType());
        }

        [Test]
        public void Checkout_CallsService()
        {
            // Arrange
            Type expected = typeof(Decimal);
            // Act
            var actual = client.Checkout();
            // Assert
            mockBasketService.Verify(m => m.CalculateBasketTotal(client.Basket), Times.Once);
        }

        [Test]
        public void ChangeSpecialOffer_ReturnsDecimal()
        {
            // Arrange
            Type expected = typeof(Boolean);
            // Act
            var actual = client.ChangeSpecialOffer(null, null);
            // Assert
            Assert.AreEqual(expected, actual.GetType());
        }

        [Test]
        public void ChangeSpecialOffer_CallsService()
        {
            // Arrange
            string sku = "mockSKU";
            SpecialOffer specialOffer = new SpecialOffer(1, 3.0m);
            // Act
            var actual = client.ChangeSpecialOffer(sku, specialOffer);
            // Assert
            mockBasketService.Verify(m => m.ChangeSpecialOffer(sku, specialOffer), Times.Once);
        }

        [Test]
        public void ChangeSpecialOffer_ReturnsfalseWhenSkuIsNull()
        {
            // Arrange
            bool expected = false;
            string sku = null;
            SpecialOffer specialOffer = new SpecialOffer(1, 3.0m);
            // Act
            var actual = client.ChangeSpecialOffer(sku, specialOffer);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ChangeSpecialOffer_ReturnsfalseWhenServiceCallReturnsFalse()
        {
            // Arrange
            bool expected = false;
            string sku = "mockSKU";
            SpecialOffer specialOffer = new SpecialOffer(1, 3.0m);
            mockBasketService.Setup(m => m.ChangeSpecialOffer(sku, specialOffer)).Returns(false);
            // Act
            var actual = client.ChangeSpecialOffer(sku, specialOffer);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ChangeSpecialOffer_ReturnsTrueWhenProcessedCorrectly()
        {
            // Arrange
            bool expected = true;
            string sku = "mockSKU";
            SpecialOffer specialOffer = new SpecialOffer(1, 3.0m);
            mockBasketService.Setup(m => m.GetProducts()).Returns(new List<Product>() { new Product(sku, "", 0m, null)});
            client.GetItems();
            mockBasketService.Setup(m => m.ChangeSpecialOffer(sku, specialOffer)).Returns(true);
            // Act
            var actual = client.ChangeSpecialOffer(sku, specialOffer);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Scan_CallsService()
        {
            // Arrange
            string sku = "mockSKU";
            // Act
            client.Scan(sku);
            // Assert
            mockBasketService.Verify(m => m.GetProductById(sku), Times.Once);
        }

        [Test]
        public void Scan_DoesNotAddToBasketWhenServiceReturnsNull()
        {
            // Arrange
            string sku = "mockSKU";
            mockBasketService.Setup(m => m.GetProductById(sku)).Returns<IBasketService>(null);
            // Act
            client.Scan(sku);
            // Assert
            Assert.AreEqual(client.Basket.Count, 0);
        }

        [Test]
        public void Scan_AddsToBasketWhenServiceReturnsSomething()
        {
            // Arrange
            string sku = "mockSKU";
            Product returnProduct = new Product(null, null, 0m, null);
            mockBasketService.Setup(m => m.GetProductById(sku)).Returns(returnProduct);
            // Act
            client.Scan(sku);
            // Assert
            Assert.AreEqual(client.Basket.Count, 1);
        }
    }
}
