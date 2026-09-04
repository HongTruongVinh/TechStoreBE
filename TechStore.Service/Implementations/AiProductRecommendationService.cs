using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Data.Repositories.Interfaces;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Ai;
using TechStore.Service.Interfaces;

namespace TechStore.Service.Implementations
{
    public class AiProductRecommendationService : IAiProductRecommendationService
    {
        private readonly IAiService _aiService;
        private readonly IUnitOfWork _uow;

        public AiProductRecommendationService(
            IAiService aiService,
            IUnitOfWork uow)
        {
            _aiService = aiService;
            _uow = uow;
        }

        public async Task<ServiceResult<ProductRecommendationResponse>>ProcessUserMessageAsync(string message, CancellationToken cancellationToken = default)
        {
            // 1. Extract customer's requirements
            var criteria = await _aiService.ExtractProductSearchCriteriaAsync(message, cancellationToken);

            if (criteria == null)
            {
                return ServiceResult<ProductRecommendationResponse>.Fail(EErrorType.NotFound, AiMessenger.NotFoundProduct);
            }

            if (!criteria.IsProductRelated)
            {
                var x = new ProductRecommendationResponse
                {
                    Summary =
                        "Xin lỗi, tôi chỉ có thể hỗ trợ bạn tìm kiếm " +
                        "và tư vấn sản phẩm tại TechStore.",

                    Recommendations = []
                };
                return ServiceResult<ProductRecommendationResponse>.Success(x);
            }

            // 2. Search products from database
            var products = await _uow.Products.SearchForAiAsync(criteria, cancellationToken);


            // 3. Ask Gemini to recommend from candidates
            var recommendation = await _aiService.GenerateProductRecommendationAsync(
                    message,
                    products,
                    cancellationToken);

            // 4. Validate Gemini's ProductIds
            var validProductIds = products
                .Select(x => x.ProductId)
                .ToHashSet();

            recommendation.Recommendations =
                recommendation.Recommendations
                    .Where(x =>
                        validProductIds.Contains(x.ProductId))
                    .OrderBy(x => x.Rank)
                    .Take(3)
                    .ToList();

            return ServiceResult<ProductRecommendationResponse>.Success(recommendation);
        }
    }
}
