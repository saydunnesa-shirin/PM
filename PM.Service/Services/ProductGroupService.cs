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
        public async Task<List<ProductGroupResponse>> GetProductGroups(SearchProductGroupQuery query)
        {
            var productGroupResponses = new List<ProductGroupResponse>();

            var productGroups = await _productGroupRepository.GetAll();

            if (query.ProductGroupId != null)
            {
                productGroups = productGroups.Where(q => q.ProductGroupId == query.ProductGroupId).ToList();
            }
            else if (query.ProductGroupName != null)
            {
                productGroups = productGroups.Where(q => q.Name == query.ProductGroupName).ToList();
            }

            if(productGroups.Count > 0)
            {
                foreach (var productGroup in productGroups)
                {
                    var productGroupResponse = _mapper.Map<ProductGroupResponse>(productGroup);
                    productGroupResponses.Add(productGroupResponse);
                }
            }

            return productGroupResponses;
        }
    }
}
