using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public async Task AddProduct(AddProductCommand command)
        {
            try
            {
                await _productService.AddProduct(command);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding product", ex.Message);
                // TODO: return custom exception message to user with error code
            }
        }

        [HttpGet("get")]
        public async Task<List<ProductResponse>> GetProducts(SearchProductQuery query)
        {
            try
            {
                return await _productService.GetProducts(query);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error finding product", ex.Message);
                throw;
            }
        }
    }
}
