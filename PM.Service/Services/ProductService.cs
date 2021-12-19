using AutoMapper;
using Microsoft.Extensions.Logging;
using PM.Common.Commands;
using PM.Common.Exceptions;
using PM.Common.Queries;
using PM.Common.Responses;
using PM.Repository.Models;
using PM.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;
        private readonly IProductGroupRepository _productGroupRepository;

        public ProductService(IProductRepository productRepository, IStoreRepository storeRepository, ILogger<ProductService> logger, IMapper mapper,
            IProductGroupRepository productGroupRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _storeRepository = storeRepository ?? throw new ArgumentNullException(nameof(storeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _productGroupRepository = productGroupRepository ?? throw new ArgumentNullException(nameof(productGroupRepository));
        }
        /// <summary>
        /// Add Product and Store
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> AddProductAsync(AddProductCommand command)
        {
            await ValidateProductGroup(command);

            var dbModel = _mapper.Map<Product>(command);
            dbModel.Stores = new List<Store>();

            foreach (var storeId in command.StoreIds)
            {
                var existingStore = await _storeRepository.GetAsync(storeId);
                if (existingStore == null)
                {
                    string message = $"Provided store id does not exist: {storeId}";
                    _logger.LogWarning(message);
                    throw new KeyNotFoundException(message);
                }
                dbModel.Stores.Add(existingStore);
            }

            CalculatePriceAndVat(ref dbModel);

            if (dbModel.Price > 0 && dbModel.PriceWithVat > 0)
            {
                return await _productRepository.AddAsync(dbModel);
            }
            else
            {
                string message = "Invalid product Price";
                _logger.LogWarning(message);
                throw new AppException(message);
            }
        }

        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<ProductResponse>> GetProductsAsync(SearchProductQuery query)
        {
            var productResponses = new List<ProductResponse>();

            if (query.ProductId == null)
            {
                if (query.ProductGroupId == null)
                {
                    var products = await _productRepository.GetAllAsync();
                    foreach (var product in products)
                    {
                        var productResponse = _mapper.Map<ProductResponse>(product);
                        productResponses.Add(productResponse);
                    }
                }
                else
                {
                    var productsByGroup = await _productRepository.GetByGroupIdAsync(query.ProductGroupId);

                    if (productsByGroup == null || productsByGroup.Count == 0)
                    {
                        string message = $"No product found for product group id: {query.ProductGroupId}";
                        _logger.LogWarning(message);
                        throw new KeyNotFoundException(message);
                    }
                    foreach (var product in productsByGroup)
                    {
                        var productResponse = _mapper.Map<ProductResponse>(product);
                        productResponses.Add(productResponse);
                    }
                }
            }
            else
            {
                var product = await _productRepository.GetAsync(query.ProductId);
                var productResponse = _mapper.Map<ProductResponse>(product);

                if (productResponse == null)
                {
                    string message = $"Product not found for id: {query.ProductId}";
                    _logger.LogWarning(message);
                    throw new KeyNotFoundException(message);
                }

                productResponses.Add(productResponse);
            }

            return productResponses;
        }

        /// <summary>
        /// Price, vat_rate, and price_with_vat, if any two values are given, then the third one calculate automatically
        /// 1st IF, Calculate price_with_vat based on  product_price and vat_rate
        /// 2nd IF, Calculate vat_rate based on  product_price and price_with_vat
        /// 3rd IF, Calculate product_price based on vat_rate and price_with_vat
        /// </summary>
        /// <param name="product">Get Product referance</param>
        public void CalculatePriceAndVat(ref Product product)
        {
            if (product.Price > 0 && product.PriceWithVat > 0)
                product.VatRate = Convert.ToInt32((product.PriceWithVat - product.Price) * 100 / product.Price);

            else if (product.PriceWithVat > 0 && product.VatRate > 0)
            {
                decimal vr = (Convert.ToDecimal(product.VatRate) / 100) + 1;
                product.Price = Math.Round((product.PriceWithVat / vr), 2);
            }
            else if (product.Price > 0 && product.VatRate >= 0)
            {
                if (product.VatRate > 0)
                    product.PriceWithVat = Math.Round((product.Price * product.VatRate) / 100 + product.Price, 2);
                else
                    product.PriceWithVat = product.Price;
            }
            else
            {
                product.VatRate = 0; product.Price = product.PriceWithVat = 0.0m;
            }
        }
        /// <summary>
        /// Check Product group exist or not
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private async Task ValidateProductGroup(AddProductCommand command)
        {
            var dbProductGroup = await _productGroupRepository.GetByIdAsync(command.ProductGroupId);
            if (dbProductGroup == null || dbProductGroup.Count == 0)
            {
                var messae = "Product group does not exist! Please correct product group Id.";
                _logger.LogWarning(messae);
                throw new KeyNotFoundException(messae);
            }
        }
    }
}
