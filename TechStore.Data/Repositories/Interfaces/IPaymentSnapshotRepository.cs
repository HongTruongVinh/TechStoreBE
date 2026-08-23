using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;

namespace TechStore.Data.Repositories.Interfaces
{
    public interface IPaymentSnapshotRepository : IRepository<PaymentSnapshot>
    {
        Task<PaymentSnapshot?> GetWithItemsAsync(string publicId);
        Task<PaymentSnapshot?> GetForUpdateAsync_PostgreSQL(string publicId, CancellationToken cancellationToken = default);
        Task<List<PaymentSnapshot>> GetExpiredPendingSnapshotsAsync(DateTime now, CancellationToken cancellationToken);
    }
}
