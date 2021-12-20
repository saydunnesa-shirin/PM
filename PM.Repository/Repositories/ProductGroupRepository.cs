using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PM.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PM.Repository.Repositories
{
    public class ProductGroupRepository : IProductGroupRepository
    {
        private readonly ProductContext _dbContext;
        private readonly ILogger<ProductGroupRepository> _logger;

        public ProductGroupRepository(ProductContext dbContext, ILogger<ProductGroupRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<ProductGroup>> GetByIdAsync(int? id)
        {
            return await _dbContext.ProductGroups
                .Where(x => x.ProductGroupId == id)
                .Include(x => x.Parent)
                .Include(x => x.Children)
                .ToListAsync();
        }

        public async Task<List<ProductGroup>> GetAllAsync()
        {
            return await _dbContext.ProductGroups
                .Include(x => x.Parent)
                .Include(x => x.Children)
                .ToListAsync();
        }
    }
}
