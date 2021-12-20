using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PM.Common.Queries;
using PM.Common.Responses;
using PM.Repository.Models;
using PM.Repository.Repositories;
using PM.Service.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM.Service.Test
{
    [TestFixture]
    public class ProductGroupServiceTest
    {
        private readonly IProductGroupService _productGroupService;
        private readonly Mock<IProductGroupRepository> _mockProductGroupRepository;
        private readonly Mock<IMapper> _mapper;

        public ProductGroupServiceTest()
        {
            var productServiceLogger = new Mock<ILogger<ProductGroupService>>();
            _mockProductGroupRepository = new Mock<IProductGroupRepository>();
            _mapper = new Mock<IMapper>();

            _productGroupService = new ProductGroupService(_mockProductGroupRepository.Object, productServiceLogger.Object, _mapper.Object);
        }

        [SetUp]
        public void Setup()
        {
            _mapper.Reset();
            _mockProductGroupRepository.Reset();
        }

        #region Get Product Group
        [Test]
        public async Task GetProductGroupsAsync()
        {
            //Arrange
            SearchProductGroupQuery query = SetUpSearchProductGroupQuery();
            List<ProductGroup> testProductGroups = SetUpTestProductGroups();
            SetUpProductGroupResponsesMapper(testProductGroups);
            _mockProductGroupRepository.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(testProductGroups));

            ////Act
            var productGroups = await _productGroupService.GetProductGroupsAsync(query);

            ////Assert
            _mockProductGroupRepository.Verify(x => x.GetAllAsync(), Times.Once);
        }

        #endregion

        #region Private Methods

        /// Get
        private SearchProductGroupQuery SetUpSearchProductGroupQuery()
        {
            return new SearchProductGroupQuery()
            {
                ProductGroupId = 1
            };
        }

        private List<ProductGroup> SetUpTestProductGroups()
        {
            return new List<ProductGroup>
            {
                new ProductGroup()
                {
                    Name = "Food",
                    ProductGroupId = 1,
                    Parent = null,
                    Children = new List<ProductGroup>
                    {
                        new ProductGroup
                        {
                            Name = "Bread",
                            ProductGroupId = 2,
                            Children = null
                        }
                    }
                }
            };
        }

        private void SetUpProductGroupResponsesMapper(List<ProductGroup> productGroups)
        {
            foreach (var item in productGroups)
            {
                _mapper.Setup(x => x.Map<ProductGroupResponse>(item)).Returns
               (
                   new ProductGroupResponse
                   {
                       ProductGroupId = item.ProductGroupId,
                       GroupName = item.Name
                   }
               );
            }
        }

        #endregion
    }
}