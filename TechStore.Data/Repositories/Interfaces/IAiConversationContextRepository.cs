using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;

namespace TechStore.Data.Repositories.Interfaces
{
    public interface IAiConversationContextRepository : IRepository<AiConversationContext>
    {
        Task<AiConversationContext?> GetByConversationIdAsync(Guid conversationId, CancellationToken cancellationToken = default);
    }
}
