using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PM.Api.Validation;
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

        [HttpPost("get")]
        public async Task<List<ProductGroupResponse>> GetProductGroups(SearchProductGroupQuery query)
        {
            SearchProductGroupQueryValidator validation = new SearchProductGroupQueryValidator();
            validation.ValidateAndThrow(query);

            _logger.LogInformation("Product group search initiated.");
            return await _productGroupService.GetProductGroupsAsync(query);
        }
    }
}
