using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PM.Api.Validation;
using PM.Common.Commands;
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
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [HttpPost("add")]
        public async Task AddProductAsync(AddProductCommand command)
        {
            AddProductCommandValidator validation = new AddProductCommandValidator();
            validation.ValidateAndThrow(command);

            _logger.LogInformation("Product add initiated.");
            var result = await _productService.AddProductAsync(command);

            if (result > 0)
                _logger.LogInformation("Product add completed.");
        }

        [HttpPost("get")]
        public async Task<List<ProductResponse>> GetProductsAsync(SearchProductQuery query)
        {
            SearchProductQueryValidator validation = new SearchProductQueryValidator();
            validation.ValidateAndThrow(query);

            _logger.LogInformation("Product search initiated.");
            return await _productService.GetProductsAsync(query);
        }
    }
}
