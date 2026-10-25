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
    public class PaymentTransactionRepository : Repository<PaymentTransaction>, IPaymentTransactionRepository
    {
        public PaymentTransactionRepository(AppDbContext context) : base(context) { }

        public async Task<PaymentTransaction?> GetExistingTransactionAsync(string transactionId)
        {
            var existingTransaction = await _dbSet.AsNoTracking()
                .Where(x => x.TransactionId == transactionId)
                .Include(x => x.PaymentSnapshot)
                .FirstOrDefaultAsync();
             
            return existingTransaction;
        }
    }
}
