using PM.Common.Commands;
using PM.Common.Queries;
using PM.Common.Responses;
using PM.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM.Service.Services
{
    public interface IProductGroupService
    {
        Task<List<ProductGroupResponse>> GetProductGroups(SearchProductGroupQuery query);
    }
}