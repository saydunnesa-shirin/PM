using PM.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM.Repository.Repositories
{
    public interface IProductGroupRepository
    {
        Task<List<ProductGroup>> GetByIdAsync(int? id);
        Task<List<ProductGroup>> GetAllAsync();
    }
}