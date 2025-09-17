using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Context;
using TechStore.Data.Entities;
using TechStore.Data.Repositories.Interfaces;

namespace TechStore.Data.Repositories.Implementations
{
    public class ProductVariantRepository : Repository<ProductVariant>, IProductVariantRepository
    {
        public ProductVariantRepository(AppDbContext context) : base(context) { }
    }
}
