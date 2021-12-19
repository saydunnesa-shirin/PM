using PM.Common.Commands;
using PM.Common.Queries;
using PM.Common.Responses;
using PM.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM.Service.Services
{
    public interface IProductService
    {
        public Task<int> AddProductAsync(AddProductCommand dto);
        Task<List<ProductResponse>> GetProductsAsync(SearchProductQuery query);
        public void CalculatePriceAndVat(ref Product product);
    }
}