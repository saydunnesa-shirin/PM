using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PM.Common.Commands;
using PM.Common.Exceptions;
using PM.Common.Queries;
using PM.Common.Responses;
using PM.Repository.Models;
using PM.Repository.Repositories;
using PM.Service.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM.Service.Test
{
    [TestFixture]
    public class ProductServiceTest
    {
        private readonly IProductService _productService;
        private readonly Mock<IStoreRepository> _mockStoreRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IProductGroupRepository> _mockProductGroupRepository;
        private readonly Mock<IMapper> _mapper;

        public ProductServiceTest()
        {
            var productServiceLogger = new Mock<ILogger<ProductService>>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockStoreRepository = new Mock<IStoreRepository>();
            _mockProductGroupRepository = new Mock<IProductGroupRepository>();
            _mapper = new Mock<IMapper>();

            _productService = new ProductService(_mockProductRepository.Object, _mockStoreRepository.Object,
                productServiceLogger.Object, _mapper.Object, _mockProductGroupRepository.Object);
        }

        [SetUp]
        public void Setup()
        {
            _mapper.Reset();
            _mockProductGroupRepository.Reset();
            _mockStoreRepository.Reset();
            _mockProductRepository.Reset();
        }

        #region Add Product Calculate Price
        [Test]
        public void CalculatePrice_GetPriceWithVat_ForPriceAndVatRate()
        {
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

        #endregion

        #region Add Product
        [Test]
        public async Task AddProductAsync()
        {
            //Arrange
            AddProductCommand command = SetUpAddProductCommand();
            SetUpMapper(command);
            _mockStoreRepository.Setup(x => x.GetAsync(It.IsAny<int>())).Returns(Task.FromResult(new Store()));

            _mockProductGroupRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new List<ProductGroup> { new ProductGroup() }));

            //Act
            var addedProduct = await _productService.AddProductAsync(command);

            //Assert
            _mockProductGroupRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _mockStoreRepository.Verify(x => x.GetAsync(It.IsAny<int>()), Times.Exactly(command.StoreIds.Count));
            _mockProductRepository.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once);
            _mockProductRepository.VerifyNoOtherCalls();
        }
        
        [Test]
        public void AddProduct_Fails_WhenProductGroupDoesNotExistAsync()
        {
            //Arrange
            AddProductCommand command = SetUpAddProductCommand();
            SetUpMapper(command);
            _mockStoreRepository.Setup(x => x.GetAsync(It.IsAny<int>())).Returns(Task.FromResult(new Store()));
            _mockProductGroupRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new List<ProductGroup>()));

            //Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _productService.AddProductAsync(command));
            Assert.That(ex.Message == "Product group does not exist! Please correct product group Id.");
        }

        [Test]
        public void AddProduct_Fails_WhenAnyStoreDoesNotExistAsync()
        {
            //Arrange
            AddProductCommand command = SetUpAddProductCommand();
            SetUpMapper(command);
            _mockStoreRepository.Setup(x => x.GetAsync(command.StoreIds[0])).Returns(Task.FromResult(new Store()));
            _mockStoreRepository.Setup(x => x.GetAsync(command.StoreIds[1])).Returns(Task.FromResult(new Store()));
            _mockProductGroupRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new List<ProductGroup> { new ProductGroup() }));

            //Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _productService.AddProductAsync(command));
            Assert.That(ex.Message == $"Provided store id does not exist: {command.StoreIds[2]}");
        }

        [Test]
        public void AddProduct_Fails_WhenInvalidProductPrices()
        {
            //Arrange
            AddProductCommand command = SetUpAddProductCommand();
            command.Price = 0;
            command.PriceWithVat = 0;
            SetUpMapper(command);
            _mockStoreRepository.Setup(x => x.GetAsync(It.IsAny<int>())).Returns(Task.FromResult(new Store()));

            _mockProductGroupRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new List<ProductGroup> { new ProductGroup() }));

            //Assert
            var ex = Assert.ThrowsAsync<AppException>(async () => await _productService.AddProductAsync(command));
            Assert.That(ex.Message == "Invalid product Price");
        }
        #endregion

        #region Get Product
        [Test]
        public async Task GetProductsAsync_ReturnSingleProduct_WhenProductIdFound()
        {
            //Arrange
            SearchProductQuery query = SetUpSearchProductQuery();
            Product testProduct = SetUpTestProducts()[0];
            SetUpProductResponsesMapper(testProduct);
            _mockProductRepository.Setup(x => x.GetAsync(It.IsAny<int>())).Returns(Task.FromResult(testProduct));

            ////Act
            var product = await _productService.GetProductsAsync(query);

            ////Assert
            _mockProductRepository.Verify(x => x.GetAsync(It.IsAny<int>()), Times.Once);
            _mockProductRepository.VerifyNoOtherCalls();
        }

        [Test]
        public void GetProducts_Fails_WhenProductDoesNotExist()
        {
            //Arrange
            SearchProductQuery query = SetUpSearchProductQuery();
            Product testProduct = SetUpTestProducts()[0];
            SetUpProductResponsesMapper(testProduct);

            //Act and Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _productService.GetProductsAsync(query));
            Assert.That(ex.Message == $"Product not found for id: {query.ProductId}");
        }

        [Test]
        public async Task GetProductsAsync_All_WhenProductIdOrProductGroupIdNotProvided()
        {
            //Arrange
            SearchProductQuery query = SetUpSearchProductQuery();
            query.ProductId = null;
            query.ProductGroupId = null;
            List<Product> testProductList = SetUpTestProducts();

            foreach (var item in testProductList)
            {
                SetUpProductResponsesMapper(item);
            }
            
            _mockProductRepository.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(testProductList));

            ////Act
            var product = await _productService.GetProductsAsync(query);

            ////Assert
            _mockProductRepository.Verify(x => x.GetAllAsync(), Times.Once);
            _mockProductRepository.VerifyNoOtherCalls();
        }

        [Test]
        public async Task GetProductsAsync_ReturnMultipleProduct_WhenProductGroupIdFound()
        {
            //Arrange
            SearchProductQuery query = SetUpSearchProductQuery();
            query.ProductId = null;
            List<Product> testProductList = SetUpTestProducts();

            foreach (var item in testProductList)
            {
                SetUpProductResponsesMapper(item);
            }

            _mockProductRepository.Setup(x => x.GetByGroupIdAsync(It.IsAny<int>())).Returns(Task.FromResult(testProductList));

            ////Act
            var product = await _productService.GetProductsAsync(query);

            ////Assert
            _mockProductRepository.Verify(x => x.GetByGroupIdAsync(It.IsAny<int>()), Times.Once);
            _mockProductRepository.VerifyNoOtherCalls();
        }

        [Test]
        public void GetProducts_Fails_WhenProductGroupIdDoesNotExist()
        {
            //Arrange
            SearchProductQuery query = SetUpSearchProductQuery();
            query.ProductId = null;
            query.ProductGroupId = 500000;

            _mockProductRepository.Setup(x => x.GetByGroupIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new List<Product>()));

            //Act and Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _productService.GetProductsAsync(query));
            Assert.That(ex.Message == $"No product found for product group id: {query.ProductGroupId}");
        }

        #endregion

        #region Private Methods

        private void SetUpMapper(AddProductCommand command)
        {
            _mapper.Setup(x => x.Map<Product>(command)).Returns
                (
                    new Product
                    {
                        Name = command.Name,
                        EntryTime = command.EntryTime,
                        Price = command.Price,
                        VatRate = command.VatRate,
                        PriceWithVat = command.PriceWithVat,
                        ProductGroupId = command.ProductGroupId,

                    }
                );
        }

        private AddProductCommand SetUpAddProductCommand()
        {
            return new AddProductCommand()
            {
                Name = "Product1",
                EntryTime = DateTime.UtcNow,
                ProductGroupId = 4,
                Price = 10,
                VatRate = 20,
                PriceWithVat = 12,
                StoreIds = new List<int> { 1, 2, 3 }
            };
        }

        /// Get
        private SearchProductQuery SetUpSearchProductQuery()
        {
            return new SearchProductQuery()
            {
                ProductId = 1,
                ProductGroupId = 4,
            };
        }

        private List<Product> SetUpTestProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Name = "Product1",
                    EntryTime = DateTime.UtcNow,
                    ProductGroupId = 4,
                    Price = 10,
                    VatRate = 20,
                    PriceWithVat = 12,
                    Stores = new List<Store> { new Store() }
                },
                new Product()
                {
                    Name = "Product2",
                    EntryTime = DateTime.UtcNow,
                    ProductGroupId = 1,
                    Price = 100,
                    VatRate = 20,
                    PriceWithVat = 120,
                    Stores = new List<Store> { new Store() }
                },
                new Product()
                {
                    Name = "Product3",
                    EntryTime = DateTime.UtcNow,
                    ProductGroupId = 3,
                    Price = 200,
                    VatRate = 20,
                    PriceWithVat = 240,
                    Stores = new List<Store> { new Store() }
                }


            };
        }

        private void SetUpProductResponsesMapper(Product product)
        {
            _mapper.Setup(x => x.Map<ProductResponse>(product)).Returns
                (
                    new ProductResponse
                    {
                        Name = product.Name,
                        EntryTime = product.EntryTime,
                        Price = product.Price,
                        VatRate = product.VatRate,
                        PriceWithVat = product.PriceWithVat,
                        ProductGroupName = "TestProductGroup",
                        Stores = new List<StoreResponse> 
                        { 
                            new StoreResponse { StoreName = "TestStore" } 
                        }
                    }
                );
        }

        #endregion
    }
}