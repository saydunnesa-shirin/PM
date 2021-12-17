using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PM.Repository.Models;
using System;
using System.Threading.Tasks;

namespace PM.Repository.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly ProductContext _dbContext;
        private readonly ILogger<StoreRepository> _logger;

        public StoreRepository(ProductContext dbContext, ILogger<StoreRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Add(Store model)
        {
            _dbContext.Stores.Add(model);
            _logger.LogInformation("Adding store to database");
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<Store> Get(int id)
        {
            return await _dbContext.Stores.SingleAsync(x => x.StoreId == id);
        }
    }
}