using PM.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM.Repository.Repositories
{
    public interface IProductGroupRepository
    {
        Task<List<ProductGroup>> GetById(int? id);
        Task<List<ProductGroup>> GetAll();
    }
}