using Microsoft.EntityFrameworkCore;
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
    public class PaymentSnapshotRepository : Repository<PaymentSnapshot>, IPaymentSnapshotRepository
    {
        public PaymentSnapshotRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PaymentSnapshot?> GetWithItemsAsync(string publicId)
        {
            return await _dbSet.Where(ps => ps.PublicId == publicId)
                .Include(ps => ps.Items)
                .FirstOrDefaultAsync();
        }
    }
}
