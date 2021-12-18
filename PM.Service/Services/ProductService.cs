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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IStoreRepository storeRepository, ILogger<ProductService> logger, IMapper mapper)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _storeRepository = storeRepository ?? throw new ArgumentNullException(nameof(storeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        /// <summary>
        /// Add Product and Store
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> AddProduct(AddProductCommand command)
        {
            var dbModel = _mapper.Map<Product>(command);

            dbModel.Stores = new List<Store>();

            foreach (var storeId in command.StoreIds)
            {
                var existingStore = await _storeRepository.Get(storeId); //TODO: validation for existance store needed
                if (existingStore == null)
                {
                    _logger.LogError($"Provided store id does not exist {storeId}");
                    throw new Exception("Provided store id does not exist");
                }

                dbModel.Stores.Add(existingStore);
            }

            CalculatePriceAndVat(ref dbModel);

            if(dbModel.Price > 0 && dbModel.PriceWithVat > 0)
            {
                return await _productRepository.Add(dbModel);
            }
            else
            {
                _logger.LogError($"Invalid product Price {dbModel.Price}.");
                throw new Exception("Invalid product Price.");
            }
        }

        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<ProductResponse>> GetProducts(SearchProductQuery query)
        {
            var productResponses = new List<ProductResponse>();

            if (query.ProductId == null)
            {
                if(query.ProductGroupId == null)
                {
                    var products = await _productRepository.GetAll();
                    foreach (var product in products)
                    {
                        var productResponse = _mapper.Map<ProductResponse>(product);
                        productResponses.Add(productResponse);
                    }
                }
                else
                {
                    var productsByGroup = await _productRepository.GetByGroupId(query.ProductGroupId);
                    foreach (var product in productsByGroup)
                    {
                        var productResponse = _mapper.Map<ProductResponse>(product);
                        productResponses.Add(productResponse);
                    }
                }
            }
            else
            {
                var product = await _productRepository.Get(query.ProductId);
                var productResponse = _mapper.Map<ProductResponse>(product);

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
                product.Price = Math.Round((product.PriceWithVat / vr),2);
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

        #region M Map
        private static Product MapToEntity(AddProductCommand commmand)
        {
            //TODO: Automapper
            //TODO: Price, PricewithVat, Vat calculation
            return new Product
            {
                EntryTime = commmand.EntryTime,
                Name = commmand.Name,
                Price = commmand.Price,
                VatRate = commmand.VatRate,
                PriceWithVat = commmand.PriceWithVat,
                ProductGroupId = commmand.ProductGroupId
            };
        }

        private static ProductResponse MapToResponse(Product product)
        {
            List<StoreResponse> storeResponses = new List<StoreResponse>();
            var stores = product.Stores.ToList();
            foreach (var item in stores)
            {
                storeResponses.Add
                    (
                        new StoreResponse { StoreName = item.Name }
                    );
            }
            return new ProductResponse
            {
                EntryTime = product.EntryTime,
                Name = product.Name,
                Price = product.Price,
                PriceWithVat = product.PriceWithVat,
                VatRate = product.VatRate,
                ProductGroupName=product.ProductGroup.Name,
                Stores= storeResponses
            };
        }

        #endregion
    }
}
