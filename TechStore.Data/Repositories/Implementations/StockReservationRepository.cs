using Microsoft.EntityFrameworkCore;
using TechStore.Data.Context;
using TechStore.Data.Entities;
using TechStore.Data.Repositories.Interfaces;

namespace TechStore.Data.Repositories.Implementations
{
    public class StockReservationRepository : Repository<StockReservation>, IStockReservationRepository
    {
        public StockReservationRepository(AppDbContext context) : base(context) { }

        public async Task<List<StockReservation>> GetByPaymentSnapshotIdAsync(Guid snapshotId, CancellationToken cancellationToken)
        {
            return await _dbSet.Where(s => s.PaymentSnapshotId == snapshotId).ToListAsync(cancellationToken);
        }
    }
}
