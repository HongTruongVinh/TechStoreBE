using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Ai;
using TechStore.Service.Implementations;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private readonly IAiService _aiService;
        private readonly IAiProductRecommendationService _aiProductRecommendationService;

        public AiController(
            IAiService aiService,
            IAiProductRecommendationService aiProductRecommendationService)
        {
            _aiService = aiService;
            _aiProductRecommendationService = aiProductRecommendationService;
        }

        [HttpPost("chat")]
        public async Task<ActionResult<ApiResponse<AiResponse>>> Chat([FromBody] AiChatRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest("Message is required.");
            }

            var serviceResult = await _aiService.ChatAsync(request.Message, cancellationToken);

            return serviceResult.ToActionResult(this);
        }

        [HttpPost("recommend-products")]
        public async Task<ActionResult<ApiResponse<ProductRecommendationResponse>>> RecommendProducts([FromBody] AiChatRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest("Message is required.");
            }

            var serviceResult = await _aiProductRecommendationService.ProcessUserMessageAsync(request.Message, cancellationToken);

            var result = serviceResult.ToActionResult(this);

            return result;
        }
    }
}
