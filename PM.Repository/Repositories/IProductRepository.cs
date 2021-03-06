using PM.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM.Repository.Repositories
{
    public interface IProductRepository
    {
        Task<int> AddAsync(Product model);
        Task<Product> GetAsync(int? id);
        Task<List<Product>> GetAllAsync();
    }
}