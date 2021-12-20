using AutoMapper;
using Microsoft.Extensions.Logging;
using PM.Common.Commands;
using PM.Common.Queries;
using PM.Common.Responses;
using PM.Repository.Models;
using PM.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PM.Service.Services
{
    public class ProductGroupService : IProductGroupService
    {
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly ILogger<ProductGroupService> _logger;
        private readonly IMapper _mapper;

        public ProductGroupService(IProductGroupRepository productGroupRepository, ILogger<ProductGroupService> logger, IMapper mapper)
        {
            _productGroupRepository = productGroupRepository ?? throw new ArgumentNullException(nameof(productGroupRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get Product Group
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<ProductGroupResponse>> GetProductGroupsAsync(SearchProductGroupQuery query)
        {
            var productGroupResponses = new List<ProductGroupResponse>();

            var productGroups = await _productGroupRepository.GetAllAsync();
            productGroups = productGroups.Where(x => x.Parent is null).ToList();

            if (query.ProductGroupId != null || query.ProductGroupId > 0)
            {
                productGroups = productGroups.Where(q => q.ProductGroupId == query.ProductGroupId).ToList();
            }

            if (productGroups.Count > 0)
            {
                foreach (var productGroup in productGroups)
                {
                    var productGroupResponse = _mapper.Map<ProductGroupResponse>(productGroup);
                    productGroupResponses.Add(productGroupResponse);
                }
            }

            if (productGroups == null || productGroups.Count == 0)
            {
                string message = $"No product group found for product group id: {query.ProductGroupId}";

                _logger.LogWarning(message);
                throw new KeyNotFoundException(message);
            }

            return productGroupResponses;
        }
    }
}
