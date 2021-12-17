using PM.Repository.Models;
using System.Threading.Tasks;

namespace PM.Repository.Repositories
{
    public interface IStoreRepository
    {
        Task<int> Add(Store model);
        Task<Store> Get(int id);
    }
}