using PM.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM.Repository.Repositories
{
    public interface IProductRepository
    {
        Task<int> Add(Product model);
        Task<Product> Get(int? id);
        Task<List<Product>> GetAll();
    }
}