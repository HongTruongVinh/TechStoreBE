using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;
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

        public async Task<List<PaymentSnapshot>>GetExpiredPendingSnapshotsAsync(DateTime now, CancellationToken cancellationToken)
        {
            return await _context.PaymentSnapshots
                .AsNoTracking()
                .Include(x => x.Items)
                .Where(x =>
                    x.Status == EPaymentSnapshotStatus.PendingPayment &&
                    x.ExpiredAt <= now)
                .ToListAsync(cancellationToken);
        }

        public async Task<PaymentSnapshot?> GetForUpdateAsync_PostgreSQL(string publicId, CancellationToken cancellationToken = default)
        {
            return await _context.PaymentSnapshots
                                    .FromSqlInterpolated($@"
                                        SELECT *
                                        FROM ""PaymentSnapshots""
                                        WHERE ""PublicId"" = {publicId}
                                        FOR UPDATE")
                                    .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
