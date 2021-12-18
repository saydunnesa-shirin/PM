using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PM.Common.Queries;
using PM.Common.Responses;
using PM.Service.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductGroupController : ControllerBase
    {
        private readonly ILogger<ProductGroupController> _logger;
        private readonly IProductGroupService _productGroupService;

        public ProductGroupController(ILogger<ProductGroupController> logger, IProductGroupService productGroupService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productGroupService = productGroupService ?? throw new ArgumentNullException(nameof(productGroupService));
        }

        [HttpGet("get")]
        public async Task<List<ProductGroupResponse>> GetProductGroups(SearchProductGroupQuery query)
        {
            try
            {
                return await _productGroupService.GetProductGroups(query);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error finding productGroup", ex.Message);
                throw;
            }
        }
    }
}
