using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;

namespace TechStore.Data.Repositories.Interfaces
{
    public interface IVoucherRepository : IRepository<Voucher>
    {
        Task<Voucher?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Function for postgreSQL
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Voucher?> GetForUpdateByVoucherCodeAsync_PostgreSQL(string voucherCode);
        Task<Voucher?> GetForUpdateByIdAsync_PostgreSQL(Guid id);
    }
}
