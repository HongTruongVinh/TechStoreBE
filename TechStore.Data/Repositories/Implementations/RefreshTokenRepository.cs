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
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext context) : base(context) { }

        public async Task<RefreshToken?> GetForUpdateAsync_PostgreSQL(string tokenHash, CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens
                                    .FromSqlInterpolated($@"
                                        SELECT *
                                        FROM ""RefreshTokens""
                                        WHERE ""TokenHash"" = {tokenHash}
                                        FOR UPDATE")
                                    .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
