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
    public class ShippingDetailRepository : Repository<ShippingDetail>, IShippingDetailRepository
    {
        public ShippingDetailRepository(AppDbContext context) : base(context) { }
    }
}
