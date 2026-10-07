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
    public class IdempotencyKeyRepository : Repository<IdempotencyKey>, IIdempotencyKeyRepository
    {
        public IdempotencyKeyRepository(AppDbContext context) : base(context) { }

        public async Task DeleteExpiredAsync(DateTimeOffset now, CancellationToken cancellationToken)
        {
            //await _context.IdempotencyKeys
            //    .Where(x => x.ExpiredAt <= now)
            //    .ExecuteDeleteAsync(cancellationToken);
            throw new NotImplementedException("ExecuteDeleteAsync is not implemented in this version of EF Core. You may need to implement a custom delete logic here.");
        }
    }
}
