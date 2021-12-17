using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PM.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PM.Repository.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _dbContext;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(ProductContext dbContext, ILogger<ProductRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Add(Product model)
        {
            _dbContext.Products.Add(model);
            _logger.LogInformation("Adding product to database");
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<Product> Get(int? id)
        {
            return await _dbContext.Products
                .Where(x => x.ProductId == id)
                .Include(x => x.ProductGroup)
                .Include(x => x.Stores)
                .SingleAsync();
        }

        public async Task<List<Product>> GetAll()
        {
            //TODO: Think about paging
            return await _dbContext.Products
                .Include(x => x.ProductGroup)
                .Include(x => x.Stores)
                .ToListAsync();
        }
    }
}
