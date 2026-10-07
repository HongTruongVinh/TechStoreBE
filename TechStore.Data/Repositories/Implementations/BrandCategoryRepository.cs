using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Context;
using TechStore.Data.Entities;
using TechStore.Data.Entities.CategoryEntities;
using TechStore.Data.Repositories.Interfaces;

namespace TechStore.Data.Repositories.Implementations
{
    public class BrandCategoryRepository : Repository<BrandCategory>, IBrandCategoryRepository
    {
        public BrandCategoryRepository(AppDbContext context) : base(context) { }

        public async Task SyncFromProductsAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.ExecuteSqlRawAsync("""
            INSERT INTO "BrandCategories" ("BrandId", "CategoryId")
            SELECT DISTINCT "BrandId", "CategoryId"
            FROM "Products"
            WHERE "BrandId" IS NOT NULL
              AND "CategoryId" IS NOT NULL
            ON CONFLICT ("BrandId", "CategoryId") DO NOTHING;
            """,
           cancellationToken);
        }
    }
}
