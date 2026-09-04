using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Models;
using TechStore.Data.Repositories.QueryModels;
using TechStore.Model.DTOs.Ai;

namespace TechStore.Service.Interfaces
{
    public interface IAiService
    {
        Task<ServiceResult<AiResponse>> ChatAsync(string message, CancellationToken cancellationToken = default);

        Task<ProductSearchCriteria>ExtractProductSearchCriteriaAsync(string message, CancellationToken cancellationToken = default);

        Task<ProductRecommendationResponse>GenerateProductRecommendationAsync(
                string userQuery,
                IEnumerable<AiProductContext> products,
                CancellationToken cancellationToken = default);
    }
}
