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
    public class AiConversationContextRepository : Repository<AiConversationContext>, IAiConversationContextRepository
    {
        public AiConversationContextRepository(AppDbContext context) : base(context) { }

        public async Task<AiConversationContext?> GetByConversationIdAsync(Guid conversationId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.ConversationId == conversationId, cancellationToken);
        }
    }
}
