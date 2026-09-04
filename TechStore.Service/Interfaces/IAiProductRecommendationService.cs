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
    }
}
