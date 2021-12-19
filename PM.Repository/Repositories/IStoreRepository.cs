using PM.Repository.Models;
using System.Threading.Tasks;

namespace PM.Repository.Repositories
{
    public interface IStoreRepository
    {
        Task<int> AddAsync(Store model);
        Task<Store> GetAsync(int id);
    }
}