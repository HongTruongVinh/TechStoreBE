using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;
using TechStore.Data.Entities.CategoryEntities;

namespace TechStore.Data.Repositories.Interfaces
{
    public interface IBrandCategoryRepository : IRepository<BrandCategory>
    {
        Task SyncFromProductsAsync(CancellationToken cancellationToken = default);
    }
}
