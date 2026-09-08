using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Data.Repositories.QueryModels;
using TechStore.Model.DTOs.Ai;

namespace TechStore.Service.Interfaces
{
    public interface IAiService
    {
        Task<ServiceResult<AiResponse>> ChatAsync(string message, CancellationToken cancellationToken = default);

        Task<ProductSearchCriteria?>ExtractProductSearchCriteriaAsync(string message, AiConversationContext? context, CancellationToken cancellationToken = default);

        Task<AiResult> GenerateProductRecommendationAsync(string userQuery, IEnumerable<AiProductContext> products, string? previousInteractionId, CancellationToken cancellationToken = default);

        Task<AiResponse> SendMessageAsync(string message, string? previousInteractionId, CancellationToken cancellationToken = default);

    }
}
