using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Extensions;
using TechStore.Common.Helpers;
using TechStore.Common.Models;
using TechStore.Data.Entities;
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
            var criteria = await _aiService.ExtractProductSearchCriteriaAsync(message, null, cancellationToken);

            if (criteria == null)
            {
                return ServiceResult<ProductRecommendationResponse>.Failure(EErrorType.NotFound, AiMessenger.NotFoundProduct);
            }

            if (!criteria.IsProductRelated)
            {
                var response = new ProductRecommendationResponse
                {
                    Summary =
                        "Xin lỗi, tôi chỉ có thể hỗ trợ bạn tìm kiếm " +
                        "và tư vấn sản phẩm tại TechStore.",

                    Recommendations = []
                };
                return ServiceResult<ProductRecommendationResponse>.Success(response);
            }

            // 2. Search products from database
            var products = await _uow.Products.SearchForAiAsync(criteria, cancellationToken);


            // 3. Ask Gemini to recommend from candidates
            var aiResult = await _aiService.GenerateProductRecommendationAsync(
                    message,
                    products, null,
                    cancellationToken);

            // 4. Validate Gemini's ProductIds
            var validProductIds = products
                .Select(x => x.ProductId)
                .ToHashSet();

            aiResult.Recommendation.Recommendations =
                aiResult.Recommendation.Recommendations
                    .Where(x =>
                        validProductIds.Contains(x.ProductId))
                    .OrderBy(x => x.Rank)
                    .Take(3)
                    .ToList();

            return ServiceResult<ProductRecommendationResponse>.Success(aiResult.Recommendation);
        }

        public async Task<ServiceResult<AiChatResponse>> ProcessMessageAsync_backup(string? userId, string? guestId, string? conversationId, string message, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException(
                    "Message cannot be empty.",
                    nameof(message));

            if (userId == null && guestId == null)
            {
                throw new ArgumentException(
                    "Either userId or guestId must be provided.",
                    nameof(userId));
            }

            User? user = null;
            if (userId != null)
            {
                user = await _uow.Users.GetByIdAsync(userId, cancellationToken);
                if (user == null)
                    throw new KeyNotFoundException("User not found.");
            }

            AiConversation? conversation = null;

            // =========================================================
            // 1. Lấy conversation cũ hoặc tạo conversation mới
            // =========================================================

            if (conversationId != null)
            {
                conversation = await _uow.AiConversations.GetByIdAsync(conversationId, cancellationToken);

                if (conversation == null)
                    throw new KeyNotFoundException(
                        "Conversation not found.");

                // Không cho user truy cập conversation của người khác
                if (conversation.PublicId != userId)
                    throw new UnauthorizedAccessException(
                        "You do not have access to this conversation.");
            }
            else
            {
                if (user != null)
                {
                    conversation = new AiConversation
                    {
                        Id = Guid.NewGuid(),
                        PublicId = ShareFunctions.GenerateRandomStringId(),
                        UserId = user.Id,
                        UserPublicId = user.PublicId,
                        Title = message.Length > 100
                        ? message[..100]
                        : message,
                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                        UpdatedAt = TimeZoneHelper.GetUtcNow()
                    };

                    await _uow.AiConversations.AddAsync(
                        conversation,
                        cancellationToken);

                    await _uow.CommitAsync(cancellationToken);
                }
                else
                {
                    conversation = new AiConversation
                    {
                        Id = Guid.NewGuid(),
                        PublicId = ShareFunctions.GenerateRandomStringId(),
                        GuestId = guestId,
                        Title = message.Length > 100
                        ? message[..100]
                        : message,
                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                        UpdatedAt = TimeZoneHelper.GetUtcNow()
                    };

                    await _uow.AiConversations.AddAsync(
                        conversation,
                        cancellationToken);

                    await _uow.CommitAsync(cancellationToken);
                }
            }

            // =========================================================
            // 2. Gọi Gemini
            //
            // Nếu conversation đã có LastInteractionId,
            // Gemini sẽ tiếp tục context cũ.
            // =========================================================

            var aiResponse = await _aiService.SendMessageAsync(
                message,
                conversation!.LastInteractionId,
                cancellationToken);

            // =========================================================
            // 3. Lưu message của user
            // =========================================================

            var userMessage = new AiConversationMessage
            {
                Id = Guid.NewGuid(),
                PublicId = ShareFunctions.GenerateRandomStringId(),
                ConversationId = conversation.Id,
                Role = "user",
                Content = message,
                CreatedAt = TimeZoneHelper.GetUtcNow()
            };

            await _uow.AiConversationMessages.AddAsync(userMessage, cancellationToken);

            // =========================================================
            // 4. Lưu message của assistant
            // =========================================================

            var assistantMessage = new AiConversationMessage
            {
                Id = Guid.NewGuid(),
                PublicId = ShareFunctions.GenerateRandomStringId(),
                ConversationId = conversation.Id,
                Role = "assistant",
                Content = aiResponse.Content,
                InteractionId = aiResponse.InteractionId,
                InputTokens = aiResponse.InputTokens,
                OutputTokens = aiResponse.OutputTokens,
                CreatedAt = TimeZoneHelper.GetUtcNow()
            };

            await _uow.AiConversationMessages.AddAsync(assistantMessage, cancellationToken);

            // =========================================================
            // 5. Cập nhật context của conversation
            // =========================================================

            conversation.LastInteractionId = aiResponse.InteractionId;

            conversation.UpdatedAt = TimeZoneHelper.GetUtcNow();

            _uow.AiConversations.Update(conversation);

            await _uow.CommitAsync(cancellationToken);

            // =========================================================
            // 6. Trả response cho Controller
            // =========================================================

            var aiChatResponse = new AiChatResponse
            {
                ConversationId = conversation.PublicId,
                Content = aiResponse.Content
            };

            return ServiceResult<AiChatResponse>.Success(aiChatResponse);
        }

        public async Task<ServiceResult<AiChatResponse>> ProcessMessageAsync(string? userId, string? guestId, string? conversationId, string message, CancellationToken cancellationToken)
        {
            var systemConfigs = await _uow.SystemConfigs.TableNoTracking.FirstOrDefaultAsync(cancellationToken);
            if (systemConfigs == null)
                throw new KeyNotFoundException("System configurations not found.");
            if(systemConfigs.isAiChatbotEnabled == false)
            {
                return ServiceResult<AiChatResponse>.Success(new AiChatResponse
                {
                    ConversationId = conversationId??"",
                    Content = "Hệ thống đang tạm dừng Chatbot để bảo trì. Vui lòng quay lại sau."
                });
            }

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException(
                    "Message cannot be empty.",
                    nameof(message));

            if (userId == null && guestId == null)
            {
                guestId = Guid.NewGuid().ToString();
            }

            User? user = null;
            if (userId != null)
            {
                user = await _uow.Users.GetByIdAsync(userId, cancellationToken);
                if (user == null)
                    throw new KeyNotFoundException("User not found.");
            }

            AiConversation? conversation = null;
            AiConversationContext? conversationContext = null;

            // =========================================================
            // 1. Lấy conversation cũ hoặc tạo conversation mới
            // =========================================================

            if (!string.IsNullOrWhiteSpace(conversationId))
            {
                conversation = await _uow.AiConversations.GetByIdAsync(conversationId, cancellationToken);

                if (conversation == null)
                    throw new KeyNotFoundException(
                        "Conversation not found.");

                // Không cho user truy cập conversation của người khác // tuy nhiện tạm thời thêm conversation.UserPublicId != null tạm thời có thể lưu conversation cho guestId trước khi login, sau khi login thì sẽ lưu conversation cho userId
                if (conversation.UserPublicId != null && conversation.UserPublicId != userId)
                    throw new UnauthorizedAccessException(
                        "You do not have access to this conversation.");

                conversationContext = await _uow.AiConversationContexts.GetByConversationIdAsync(conversation.Id, cancellationToken);

                if (conversationContext == null)
                    throw new KeyNotFoundException(
                        "Conversation context not found.");

                // if user has have conversation before login -> save that conversation for this user
                if (userId != null && guestId != null)
                {
                    conversation.UserPublicId = user!.PublicId;
                    conversation.UserId = user!.Id;
                }
            }
            else
            {
                if (user != null)
                {
                    conversation = new AiConversation
                    {
                        Id = Guid.NewGuid(),
                        PublicId = ShareFunctions.GenerateRandomStringId(),
                        UserId = user.Id,
                        UserPublicId = user.PublicId,
                        Title = message.Length > 100
                        ? message[..100]
                        : message,
                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                        UpdatedAt = TimeZoneHelper.GetUtcNow()
                    };
                }
                else
                {
                    conversation = new AiConversation
                    {
                        Id = Guid.NewGuid(),
                        PublicId = ShareFunctions.GenerateRandomStringId(),
                        GuestId = guestId,
                        Title = message.Length > 100
                        ? message[..100]
                        : message,
                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                        UpdatedAt = TimeZoneHelper.GetUtcNow()
                    };
                }

                conversationContext = new AiConversationContext
                {
                    ConversationId = conversation.Id,
                    ConversationPublicId = conversation.PublicId,
                    PublicId = ShareFunctions.GenerateRandomStringId(),
                    CreatedAt = TimeZoneHelper.GetUtcNow(),
                };

                await _uow.AiConversations.AddAsync(conversation, cancellationToken);

                await _uow.AiConversationContexts.AddAsync(conversationContext, cancellationToken);

                await _uow.CommitAsync(cancellationToken);
            }

            // =========================================================
            // 3. Lưu message của user
            // =========================================================

            var userMessage = new AiConversationMessage
            {
                Id = Guid.NewGuid(),
                PublicId = ShareFunctions.GenerateRandomStringId(),
                ConversationId = conversation.Id,
                Role = "user",
                Content = message,
                CreatedAt = TimeZoneHelper.GetUtcNow()
            };

            await _uow.AiConversationMessages.AddAsync(userMessage, cancellationToken);


            var criteria = await _aiService.ExtractProductSearchCriteriaAsync(message, conversationContext, cancellationToken);

            if (criteria == null)
            {
                return ServiceResult<AiChatResponse>.Success(new AiChatResponse
                {
                    ConversationId = conversation.PublicId,
                    Content = "Bạn vui lòng cung cấp thêm thông tin về nhu cầu của bạn, như giá sản phẩm, nhu cầu học tập hay chơi game gì.",
                    Recommendations = []
                });
            }

            if (!criteria.IsProductRelated)
            {
                var response = new AiChatResponse
                {
                    ConversationId = conversation.PublicId,
                    Content =
                        "Xin lỗi, tôi chỉ có thể hỗ trợ bạn tìm kiếm " +
                        "và tư vấn sản phẩm tại TechStore. " +
                        "Bạn vui lòng cung cấp thêm thông tin về nhu cầu của bạn, như giá sản phẩm, nhu cầu học tập hay chơi game gì.",

                    Recommendations = []
                };

                return ServiceResult<AiChatResponse>.Success(response);
            }

            // 2. Search products from database
            var products = await _uow.Products.SearchForAiAsync(criteria, cancellationToken);

            if(products == null)
            {
                return ServiceResult<AiChatResponse>.Success(
                    new AiChatResponse
                    {
                        ConversationId = conversation.PublicId,
                        Content = "Xin lỗi, tôi không tìm thấy sản phẩm nào phù hợp với yêu cầu của bạn: " +
                        $"{criteria.Category} {criteria.Brand} giá từ {criteria.MinPrice} - {criteria.MaxPrice}" +
                        $"nhu cầu {string.Join(", ", criteria.Usages)}" +
                        (criteria.Games.Count > 0 ? $" {string.Join(", ", criteria.Games)}" : ""),
                        Recommendations = []
                    });
            }

            if (products.Count < 1)
            {
                return ServiceResult<AiChatResponse>.Success(
                    new AiChatResponse
                    {
                        ConversationId = conversation.PublicId,
                        Content = "Xin lỗi, tôi không tìm thấy sản phẩm nào phù hợp với yêu cầu của bạn: " +
                        $"{criteria.Category} {criteria.Brand} giá từ {ConvertData.ToStr(criteria.MinPrice??0)} - {ConvertData.ToStr(criteria.MaxPrice??0)} " +
                        $"nhu cầu {string.Join(", ", criteria.Usages)}" +
                        (criteria.Games.Count > 0 ? $" {string.Join(", ", criteria.Games)}" : ""),
                        Recommendations = []
                    });
            }


            // 3. Ask Gemini to recommend from candidates
            var aiResult = await _aiService.GenerateProductRecommendationAsync(
                    message,
                    products,
                    conversation.LastInteractionId,
                    cancellationToken);

            // =========================================================
            // 4. Lưu message của assistant
            // =========================================================

            var assistantMessage = new AiConversationMessage
            {
                Id = Guid.NewGuid(),
                PublicId = ShareFunctions.GenerateRandomStringId(),
                ConversationId = conversation.Id,
                Role = "assistant",
                Content = aiResult.Recommendation.Summary,
                InteractionId = aiResult.InteractionId,
                InputTokens = aiResult.InputTokens,
                OutputTokens = aiResult.OutputTokens,
                CreatedAt = TimeZoneHelper.GetUtcNow()
            };

            conversationContext.Brand = criteria.Brand;
            conversationContext.Category = criteria.Category;
            conversationContext.MinPrice = criteria.MinPrice;
            conversationContext.MaxPrice = criteria.MaxPrice;
            conversationContext.Usages = criteria.Usages;
            conversationContext.Games = criteria.Games;
            conversationContext.LastRecommendedProductIds = products.Select(p => p.ProductId).ToList();
            conversationContext.UpdatedAt = TimeZoneHelper.GetUtcNow();
            _uow.AiConversationContexts.Update(conversationContext);

            string title = message.Length > 100 ? message[..100] : message;

            conversation.LastInteractionId = aiResult.InteractionId; // Mỗi lần tương tác với AI sẽ có 1 InteractionId riêng nên cần phải cập nhật lại LastInteractionId sau mỗi lần AI phản hồi (để đoạn chat của AI có context)
            conversation.Title = title;
            conversation.UpdatedAt = TimeZoneHelper.GetUtcNow();
            _uow.AiConversations.Update(conversation);


            await _uow.AiConversationMessages.AddAsync(assistantMessage, cancellationToken);

            var commitResult = await _uow.CommitAsync(cancellationToken);

            if(commitResult < 1)
            {
                return ServiceResult<AiChatResponse>.Failure(EErrorType.Status500InternalServerError, "Failed to save conversation messages.");
            }

            // =========================================================
            // 6. Trả response cho Controller
            // =========================================================

            var aiChatResponse = new AiChatResponse
            {
                ConversationId = conversation.PublicId,
                GuestId = conversation.GuestId,
                Content = aiResult.Recommendation.Summary,
                Recommendations = aiResult.Recommendation.Recommendations
                    .Where(x => products.Any(p => p.ProductId == x.ProductId))
                    .OrderBy(x => x.Rank)
                    .Take(3)
                    .ToList()
            };

            return ServiceResult<AiChatResponse>.Success(aiChatResponse);
        }
    }
}
