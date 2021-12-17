using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PM.Repository.Models;
using PM.Repository.Repositories;
using PM.Service.Services;
using System;

namespace PM.Service.Test
{
    [TestFixture]
    public class ProductServiceTest
    {
        private readonly IProductService _productService;
        private readonly Mock<IStoreRepository> _mockStoreRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IMapper> _mapper;

        public ProductServiceTest()
        {
            var productServiceLogger = new Mock<ILogger<ProductService>>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockStoreRepository = new Mock<IStoreRepository>();
            _mapper = new Mock<IMapper>();

            //UserDetail im = getUserDetail(); // get value of UserDetails

            _productService = new ProductService(_mockProductRepository.Object,_mockStoreRepository.Object, 
                productServiceLogger.Object, _mapper.Object);
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CalculatePrice_GetPriceWithVat_ForPriceAndVatRate()
        {
            //_mapper.Setup(m => m.Map<UserDetailViewModel, UserDetail>(It.IsAny<UserDetailtViewModel>())).Returns(im); // mapping data
            //Arrange
            var product = new Product()
            {
                ProductId = 1,
                Name = "Test",
                EntryTime = DateTime.UtcNow,
                ProductGroupId = 4,
                Price = 250,
                VatRate = 12,
                PriceWithVat = 0
            };

            var result = 280;

            //Act
            _productService.CalculatePriceAndVat(ref product);

            //Assert
            Assert.AreEqual(result, product.PriceWithVat);
        }

        [Test]
        public void CalculatePrice_GetVatRate_ForPriceAndPriceWithVat()
        {
            //Arrange
            var product = new Product()
            {
                ProductId = 1,
                Name = "Test",
                EntryTime = DateTime.UtcNow,
                ProductGroupId = 4,
                Price = 8.99M,
                VatRate = 0,
                PriceWithVat = 10.07M
            };

            var result = 12;

            //Act
            _productService.CalculatePriceAndVat(ref product);
            
            //Assert
            Assert.AreEqual(result, product.VatRate);
        }

        [Test]
        public void CalculatePrice_GetPrice_ForVatRateAndPriceWithVat()
        {
            //Arrange
            var product = new Product()
            {
                ProductId = 1,
                Name = "Test",
                EntryTime = DateTime.UtcNow,
                ProductGroupId = 4,
                Price = 0,
                VatRate = 12,
                PriceWithVat = 10.07M
            };

            var result = 8.99M;

            //Act
            _productService.CalculatePriceAndVat(ref product);
            
            //Assert
            Assert.AreEqual(result, product.Price);
        }

        [Test]
        public void CalculatePrice_GetAllZeros_ForPriceVatRateAndPriceWithVat()
        {
            //Arrange
            var product = new Product()
            {
                ProductId = 1,
                Name = "Test",
                EntryTime = DateTime.UtcNow,
                ProductGroupId = 4,
                Price = 0,
                VatRate = 0,
                PriceWithVat = 0
            };

            var result = 0;

            //Act
            _productService.CalculatePriceAndVat(ref product);
            
            //Assert
            Assert.AreEqual(result, product.Price);
            Assert.AreEqual(result, product.VatRate);
            Assert.AreEqual(result, product.PriceWithVat);
        }
    }
}