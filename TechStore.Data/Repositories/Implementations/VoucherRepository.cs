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
    public class VoucherRepository : Repository<Voucher>, IVoucherRepository
    {
        public VoucherRepository(AppDbContext context) : base(context) { }

        public async Task<Voucher?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbSet.Where(p => p.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Voucher?> GetForUpdateByIdAsync_PostgreSQL(Guid id)
        {
            return await _context.Vouchers
                                    .FromSqlInterpolated($@"
                                        SELECT *
                                        FROM ""Vouchers""
                                        WHERE ""Id"" = {id}
                                        FOR UPDATE")
                                    .SingleOrDefaultAsync();
        }

        public async Task<Voucher?> GetForUpdateByVoucherCodeAsync_PostgreSQL(string voucherCode)
        {
            return await _context.Vouchers
                                    .FromSqlInterpolated($@"
                                        SELECT *
                                        FROM ""Vouchers""
                                        WHERE ""Code"" = {voucherCode}
                                        FOR UPDATE")
                                    .SingleOrDefaultAsync();
        }
    }
}
