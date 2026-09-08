using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Ai;

namespace TechStore.Service.Interfaces
{
    public interface IAiProductRecommendationService
    {
        Task<ServiceResult<ProductRecommendationResponse>> ProcessUserMessageAsync(
            string message,
            CancellationToken cancellationToken = default);

        public Task<ServiceResult<AiChatResponse>> ProcessMessageAsync(
            string? userId, 
            string? guestId, 
            string? conversationId,
            string message, 
            CancellationToken cancellationToken = default);
    }
}
