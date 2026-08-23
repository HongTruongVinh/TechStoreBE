using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Helpers;
using TechStore.Data.Entities;
using TechStore.Data.UnitOfWork;
using TechStore.Service.Interfaces;

namespace TechStore.Service.Implementations
{
    public class PaymentSnapshotExpirationService : IPaymentSnapshotExpirationService
    {
        private readonly IUnitOfWork _uow;

        public PaymentSnapshotExpirationService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task ExpireAsync(CancellationToken cancellationToken)
        {
            var now = TimeZoneHelper.GetUtcNow();

            var snapshots = await _uow.PaymentSnapshots.GetExpiredPendingSnapshotsAsync(now, cancellationToken);

            foreach (var snapshot in snapshots)
            {
                await ExpireSnapshotAsync(snapshot, cancellationToken);
            }
        }

        private async Task ExpireSnapshotAsync(PaymentSnapshot snapshot, CancellationToken cancellationToken)
        {
            await using var transaction = await _uow.BeginTransactionAsync();

            try
            {
                var currentSnapshot = await _uow.PaymentSnapshots
                    .GetForUpdateAsync_PostgreSQL(snapshot.PublicId, cancellationToken);

                if (currentSnapshot == null ||
                    currentSnapshot.Status != EPaymentSnapshotStatus.PendingPayment ||
                    currentSnapshot.ExpiredAt > TimeZoneHelper.GetUtcNow())
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return;
                }

                // =========================================================
                // 1. Release StockReservation
                // =========================================================

                var reservations = await _uow.StockReservations.GetByPaymentSnapshotIdAsync(currentSnapshot.Id, cancellationToken);

                if (reservations.Any())
                {
                    _uow.StockReservations.RemoveRange(reservations);
                }

                // =========================================================
                // 2. Release Voucher reservation
                // =========================================================

                if (currentSnapshot.VoucherId.HasValue)
                {
                    var voucher =
                        await _uow.Vouchers.GetByIdAsync(
                            currentSnapshot.VoucherId.Value,
                            cancellationToken);

                    if (voucher != null)
                    {
                        voucher.ReservedCount--;

                        if (voucher.ReservedCount < 0)
                        {
                            voucher.ReservedCount = 0;
                        }

                        _uow.Vouchers.Update(voucher);
                    }
                }

                // =========================================================
                // 3. Expire PaymentSnapshot
                // =========================================================

                currentSnapshot.Status = EPaymentSnapshotStatus.Expired;
                currentSnapshot.UpdatedAt = TimeZoneHelper.GetUtcNow();

                _uow.PaymentSnapshots.Update(currentSnapshot);

                // =========================================================
                // 4. Commit
                // =========================================================

                var result = await _uow.CommitAsync(cancellationToken);

                if (result < 1)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return;
                }

                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
