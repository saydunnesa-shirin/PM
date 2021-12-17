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
        public Task<int> AddProduct(AddProductCommand dto);
        Task<List<ProductResponse>> GetProducts(SearchProductQuery query);

        public void CalculatePriceAndVat(ref Product product);
    }
}