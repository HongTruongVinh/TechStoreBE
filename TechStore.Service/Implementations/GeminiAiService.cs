using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Extensions;
using TechStore.Common.Helpers;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Data.Repositories.Implementations;
using TechStore.Data.Repositories.QueryModels;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Ai;
using TechStore.Model.DTOs.Ai.Ver2;
using TechStore.Service.Interfaces;

namespace TechStore.Service.Implementations
{
    public class GeminiAiService : IAiService
    {
        private readonly HttpClient _httpClient;
        private readonly GeminiAIConfig _options;


        public GeminiAiService(
            HttpClient httpClient,
            IOptions<GeminiAIConfig> config)
        {
            _httpClient = httpClient;
            _options = config.Value;
        }

        public async Task<ServiceResult<AiResponse>> Chat2Async(string message, CancellationToken cancellationToken = default)
        {
            var request = new
            {
                model = _options.Model,
                input = message
            };

            using var response = await _httpClient.PostAsJsonAsync(
                "https://generativelanguage.googleapis.com/v1beta/interactions",
                request,
                cancellationToken);

            var responseBody = await response.Content
                .ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Gemini API request failed. " +
                    $"StatusCode: {response.StatusCode}. " +
                    $"Response: {responseBody}");
            }

            using var json = JsonDocument.Parse(responseBody);

            // Lấy output text từ response
            if (json.RootElement.TryGetProperty("outputs", out var outputs))
            {
                foreach (var output in outputs.EnumerateArray())
                {
                    if (!output.TryGetProperty("type", out var type))
                        continue;

                    if (type.GetString() != "text")
                        continue;

                    if (output.TryGetProperty("text", out var text))
                    {
                        if (text.GetString() == null)
                        {
                            return ServiceResult<AiResponse>.Failure(EErrorType.Status500InternalServerError, string.Empty);
                        }

                        var chatResponse = new AiResponse { Content = text.GetString()!, InteractionId = ShareFunctions.GenerateRandomStringId() };

                        return ServiceResult<AiResponse>.Success(chatResponse);
                    }
                }
            }

            return ServiceResult<AiResponse>.Failure(EErrorType.Status500InternalServerError, string.Empty);
        }


        public async Task<ServiceResult<AiResponse>> ChatAsync(string message, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_options.ApiKey))
            {
                throw new InvalidOperationException(
                    "Gemini API key is missing.");
            }

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://generativelanguage.googleapis.com/v1beta/interactions");

            request.Headers.Add(
                "x-goog-api-key",
                _options.ApiKey);

            var body = new
            {
                model = _options.Model,
                input = message
            };

            request.Content = JsonContent.Create(body);

            using var response = await _httpClient.SendAsync(
                request,
                cancellationToken);

            var responseBody =
                await response.Content.ReadAsStringAsync(
                    cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Gemini API request failed. " +
                    $"StatusCode: {response.StatusCode}. " +
                    $"Response: {responseBody}");
            }

            using var json =
                JsonDocument.Parse(responseBody);

            if (!json.RootElement.TryGetProperty(
                    "steps",
                    out var steps))
            {
                return ServiceResult<AiResponse>.Failure(EErrorType.Status500InternalServerError, string.Empty);
            }

            foreach (var step in steps.EnumerateArray())
            {
                if (!step.TryGetProperty(
                        "type",
                        out var type))
                {
                    continue;
                }

                if (type.GetString() != "model_output")
                {
                    continue;
                }

                if (!step.TryGetProperty(
                        "content",
                        out var content))
                {
                    continue;
                }

                foreach (var item in content.EnumerateArray())
                {
                    if (item.TryGetProperty("text", out var text))
                    {
                        if (text.GetString() == null)
                        {
                            return ServiceResult<AiResponse>.Failure(EErrorType.Status500InternalServerError, string.Empty);
                        }

                        var chatResponse = new AiResponse { Content = text.GetString()!, InteractionId = ShareFunctions.GenerateRandomStringId() };

                        return ServiceResult<AiResponse>.Success(chatResponse);
                    }
                }
            }

            return ServiceResult<AiResponse>.Failure(EErrorType.Status500InternalServerError, string.Empty);
        }

        public async Task<ProductSearchCriteria> ExtractProductSearchCriteriaAsync_backup(string message, CancellationToken cancellationToken = default)
        {
            var schema = new
            {
                type = "object",

                properties = new
                {
                    isProductRelated = new
                    {
                        type = "boolean",
                        description = "Indicates if the customer's request is related to TechStore products."
                    },

                    category = new
                    {
                        type = "string",
                        description = "Product category requested by the customer."
                    },

                    minPrice = new
                    {
                        type = "number",
                        description = "Minimum budget in Vietnamese Dong."
                    },

                    maxPrice = new
                    {
                        type = "number",
                        description = "Maximum budget in Vietnamese Dong."
                    },

                    brand = new
                    {
                        type = "string",
                        description = "Product brand requested by the customer."
                    },

                    usages = new
                    {
                        type = "array",
                        items = new
                        {
                            type = "string"
                        },
                        description = "Intended uses such as programming or gaming."
                    },

                    games = new
                    {
                        type = "array",
                        items = new
                        {
                            type = "string"
                        },
                        description = "Games the customer wants to play."
                    }
                },

                required = new[]
                {
                    "category",
                    "minPrice",
                    "maxPrice",
                    "usages",
                    "games"
                }
            };

            var requestBody = new
            {
                model = _options.Model,

                input = $"""
                You are the AI Product Assistant of TechStore.

                You ONLY help customers with:
                - product search
                - product recommendation
                - product comparison
                - product specifications
                - product usage requirements

                If the user's request is unrelated to TechStore products,
                return null values and empty arrays.

                Never answer unrelated questions.

                Extract product search requirements
                from the following customer request.

                Customer request:
                {message}

                Rules:
                - Convert Vietnamese currency to VND.
                - If the customer says 20-25 million,
                  return 20000000 and 25000000.
                - Do not invent requirements.
                - Return only the requested structured data.
                """,

                response_format = new
                {
                    type = "text",
                    mime_type = "application/json",
                    schema
                }
            };

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://generativelanguage.googleapis.com/v1beta/interactions");

            request.Headers.Add(
                "x-goog-api-key",
                _options.ApiKey);

            request.Content = JsonContent.Create(requestBody);

            using var response = await _httpClient.SendAsync(
                request,
                cancellationToken);

            var responseBody =
                await response.Content.ReadAsStringAsync(
                    cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Gemini API request failed. " +
                    $"StatusCode: {response.StatusCode}. " +
                    $"Response: {responseBody}");
            }

            var json = ExtractTextFromInteraction(responseBody);

            var result = JsonSerializer.Deserialize<ProductSearchCriteria>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (result is null)
            {
                throw new InvalidOperationException(
                    "Gemini returned an invalid product search criteria.");
            }

            return result;
        }

        public async Task<ProductSearchCriteria?> ExtractProductSearchCriteriaAsync(string message, AiConversationContext? conversationContext, CancellationToken cancellationToken = default)
        {
            var schema = new
            {
                type = "object",

                properties = new
                {
                    isProductRelated = new
                    {
                        type = "boolean",
                        description =
                            "Indicates if the customer's request is related to TechStore products."
                    },

                    category = new
                    {
                        type = "string",
                        description =
                            "Product category requested by the customer. Return null if not specified."
                    },

                    minPrice = new
                    {
                        type = "number",
                        description =
                            "Minimum budget in Vietnamese Dong. Return null if not specified."
                    },

                    maxPrice = new
                    {
                        type = "number",
                        description =
                            "Maximum budget in Vietnamese Dong. Return null if not specified."
                    },

                    brand = new
                    {
                        type = "string",
                        description =
                            "Product brand requested by the customer. Return null if not specified."
                    },

                    usages = new
                    {
                        type = "array",
                        items = new
                        {
                            type = "string"
                        },
                        description =
                            "Intended uses such as programming, office work or gaming."
                    },

                    games = new
                    {
                        type = "array",
                        items = new
                        {
                            type = "string"
                        },
                        description =
                            "Games the customer wants to play."
                    }
                },

                required = new[]
                {
            "isProductRelated",
            "category",
            "minPrice",
            "maxPrice",
            "brand",
            "usages",
            "games"
        }
            };

            // Serialize current conversation context so Gemini
            // can understand follow-up messages.
            var contextJson = conversationContext is null
                ? "{}"
                : JsonSerializer.Serialize(
                    conversationContext,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

            var requestBody = new
            {
                model = _options.Model,

                input = $"""
        You are the AI Product Assistant of TechStore.

        You ONLY help customers with:
        - product search
        - product recommendation
        - product comparison
        - product specifications
        - product usage requirements

        If the user's request is unrelated to TechStore products,
        set isProductRelated to false and return:
        - category = null
        - minPrice = null
        - maxPrice = null
        - brand = null
        - usages = []
        - games = []

        Never answer unrelated questions.

        Your task is to extract the customer's CURRENT product
        search requirements.

        IMPORTANT:
        The customer may be continuing a previous conversation.

        Previous TechStore conversation context:
        {contextJson}

        Current customer request:
        {message}

        Rules:
        1. First, determine whether the current customer request
           introduces a new product category.

        2. If the current request explicitly mentions a product
           category that is different from the previous context
           category, DO NOT use the previous context.

           Treat the current request as a NEW product search.
           Ignore all previous requirements such as:
           - previous category
           - previous price range
           - previous brand
           - previous usages
           - previous games

           Example:
           Previous context:
           category = Laptop
           maxPrice = 25000000
           brand = ASUS

           Customer:
           "Tôi muốn tìm điện thoại Samsung khoảng 15 triệu"

           Result:
           category = Phone
           minPrice = null
           maxPrice = 15000000
           brand = Samsung

           DO NOT preserve:
           category = Laptop
           maxPrice = 25000000
           brand = ASUS

        3. If the current request does NOT introduce a new category
           and is clearly a follow-up to the previous context,
           use the previous context.

        4. If the customer provides a new requirement while staying
           in the same category, update that requirement and preserve
           other previous requirements that are still relevant.

        5. If the customer modifies only part of the previous request,
           preserve the unchanged requirements.

        6. Example:
           Previous context:
           category = Laptop
           maxPrice = 25000000

           Customer:
           "Có con nào rẻ hơn không?"

           Result:
           category = Laptop
           maxPrice should be reduced based on the customer's request,
           but DO NOT invent an exact price if none can be determined.

        7. If the current request is ambiguous and does not clearly
           indicate a new category, use the previous context only if
           the request is clearly a follow-up.

        8. Convert Vietnamese currency to VND.

        9. If the customer says "20-25 triệu",
           return:
           minPrice = 20000000
           maxPrice = 25000000

        10. Do not invent requirements.

        11. If a requirement is not specified and cannot be determined
            from the current request or valid previous context,
            return null.

        12. usages and games must always be arrays.

        13. Return ONLY the structured JSON data.
        Do not include explanations or additional text.
        """,

                response_format = new
                {
                    type = "text",
                    mime_type = "application/json",
                    schema
                }
            };

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://generativelanguage.googleapis.com/v1beta/interactions");

            request.Headers.Add(
                "x-goog-api-key",
                _options.ApiKey);

            request.Content = JsonContent.Create(requestBody);

            using var response = await _httpClient.SendAsync(
                request,
                cancellationToken);

            var responseBody =
                await response.Content.ReadAsStringAsync(
                    cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Gemini API request failed. " +
                    $"StatusCode: {response.StatusCode}. " +
                    $"Response: {responseBody}");
            }

            var json = ExtractTextFromInteraction(responseBody);

            var result =
                JsonSerializer.Deserialize<ProductSearchCriteria>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (result is null)
            {
                throw new InvalidOperationException(
                    "Gemini returned an invalid product search criteria.");
            }

            return result;
        }

        public async Task<AiResult> GenerateProductRecommendationAsync(string userQuery, IEnumerable<AiProductContext> products, string? previousInteractionId, CancellationToken cancellationToken = default)
        {
            var productList = products
                .Select(product => new
                {
                    productId = product.ProductId,
                    slug = product.Slug,
                    productName = product.Name,
                    imgUrl = product.ImgUrl,
                    brand = product.Brand,
                    price = product.Price,
                    cpu = product.Cpu,
                    gpu = product.Gpu,
                    ram = product.Ram,
                    storage = product.Storage
                })
                .ToList();

            var schema = new
            {
                type = "object",

                properties = new
                {
                    summary = new
                    {
                        type = "string",
                        description =
                            "A concise explanation of the overall recommendation."
                    },

                    recommendations = new
                    {
                        type = "array",

                        items = new
                        {
                            type = "object",

                            properties = new
                            {
                                productId = new
                                {
                                    type = "string",
                                    description =
                                        "The exact ProductId from the provided product list."
                                },

                                slug = new
                                {
                                    type = "string",
                                    description =
                                        "The exact Slug from the provided product list."
                                },

                                productName = new
                                {
                                    type = "string",
                                    description =
                                        "Name of the product."
                                },

                                imgUrl = new
                                {
                                    type = "string",
                                    description =
                                        "The URL of the product's main image."
                                },

                                price = new
                                {
                                    type = "number",
                                    description =
                                        "The price of the product."
                                },

                                rank = new
                                {
                                    type = "integer",
                                    description =
                                        "Recommendation ranking. 1 is the best."
                                },

                                reason = new
                                {
                                    type = "string",
                                    description =
                                        "Why this product is suitable for the customer."
                                }
                            },

                            required = new[]
                            {
                                "productId",
                                "slug",
                                "productName",
                                "imgUrl",
                                "price",
                                "rank",
                                "reason"
                            }
                        }
                    }
                },

                required = new[]
                {
                    "summary",
                    "recommendations"
                }
            };

            var requestBody = new
            {
                model = _options.Model,

                input = $"""
            You are the AI Product Assistant of TechStore.

            Your task is to recommend products based ONLY on
            the products provided by the system.

            Customer request:
            {userQuery}

            Available products:
            {JsonSerializer.Serialize(productList)}

            Rules:
            1. Recommend only products from the provided list.
            2. Never invent a ProductId.
            3. Never invent product specifications.
            4. Never modify product prices.
            5. ProductId must exactly match one of the provided ProductId values.
            6. Slug must exactly match the Slug of the selected product.
            7. Recommend at most 3 products.
            8. Rank products from best to worst.
            9. Explain why each product matches the customer's requirements.
            10. If no product is suitable, return an empty recommendations array.
            11. Respond in Vietnamese.
            """,

                response_format = new
                {
                    type = "text",
                    mime_type = "application/json",
                    schema
                },

                // Chỉ gửi khi đang tiếp tục một conversation
                previous_interaction_id = previousInteractionId
            };

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://generativelanguage.googleapis.com/v1beta/interactions");

            request.Headers.Add(
                "x-goog-api-key",
                _options.ApiKey);

            request.Content = JsonContent.Create(requestBody);

            using var response = await _httpClient.SendAsync(
                request,
                cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Gemini API request failed. " +
                    $"StatusCode: {response.StatusCode}. " +
                    $"Response: {responseBody}");
            }

            using var document = JsonDocument.Parse(responseBody);

            var root = document.RootElement;

            var interactionId = root
                .GetProperty("id")
                .GetString();

            if (string.IsNullOrWhiteSpace(interactionId))
            {
                throw new InvalidOperationException(
                    "Gemini response does not contain a valid interaction ID.");
            }

            int? inputTokens = null;
            int? outputTokens = null;

            if (root.TryGetProperty("usage", out var usage))
            {
                if (usage.TryGetProperty("total_input_tokens", out var inputTokenElement))
                {
                    inputTokens = inputTokenElement.GetInt32();
                }

                if (usage.TryGetProperty("total_output_tokens", out var outputTokenElement))
                {
                    outputTokens = outputTokenElement.GetInt32();
                }
            }

            var jsonText = ExtractTextFromInteraction(responseBody);

            var recommendation =
                JsonSerializer.Deserialize<ProductRecommendationResponse>(
                    jsonText,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (recommendation is null)
            {
                throw new InvalidOperationException(
                    "Failed to deserialize Gemini recommendation response.");
            }

            return new AiResult
            {
                InteractionId = interactionId,
                InputTokens = inputTokens,
                OutputTokens = outputTokens,
                Recommendation = recommendation
            };
        }

        public async Task<ProductRecommendationResponse> GenerateProductRecommendationAsync_backup(string userQuery, IEnumerable<AiProductContext> products, CancellationToken cancellationToken = default)
        {
            var productList = products
                .Select((product, index) => new
                {
                    productId = product.ProductId,
                    productName = product.Name,
                    imgUrl = product.ImgUrl,
                    brand = product.Brand,
                    price = product.Price,
                    cpu = product.Cpu,
                    gpu = product.Gpu,
                    ram = product.Ram,
                    storage = product.Storage
                })
                .ToList();

            var schema = new
            {
                type = "object",

                properties = new
                {
                    summary = new
                    {
                        type = "string",
                        description =
                            "A concise explanation of the overall recommendation."
                    },

                    recommendations = new
                    {
                        type = "array",

                        items = new
                        {
                            type = "object",

                            properties = new
                            {
                                productId = new
                                {
                                    type = "string",
                                    description =
                                        "The exact ProductId from the provided product list."
                                },

                                slug = new
                                {
                                    type = "string",
                                    description =
                                        "The exact Slug from the provided product list."
                                },

                                productName = new
                                {
                                    type = "string",
                                    description =
                                        "Name of the product."
                                },

                                imgUrl = new
                                {
                                    type = "string",
                                    description =
                                        "The URL of the product's main image."
                                },

                                price = new
                                {
                                    type = "number",
                                    description =
                                        "The price of the product."
                                },

                                rank = new
                                {
                                    type = "integer",
                                    description =
                                        "Recommendation ranking. 1 is the best."
                                },

                                reason = new
                                {
                                    type = "string",
                                    description =
                                        "Why this product is suitable for the customer."
                                }
                            },

                            required = new[]
                            {
                                "productId",
                                "slug",
                                "productName",
                                "imgUrl",
                                "price",
                                "rank",
                                "reason"
                            }
                        }
                    }
                },

                required = new[]
                {
                    "summary",
                    "recommendations"
                }
            };

            var requestBody = new
            {
                model = _options.Model,

                input = $"""
            You are the AI Product Assistant of TechStore.

            Your task is to recommend products based ONLY on
            the products provided by the system.

            Customer request:
            {userQuery}

            Available products:
            {JsonSerializer.Serialize(productList)}

            Rules:
            1. Recommend only products from the provided list.
            2. Never invent a ProductId.
            3. Never invent product specifications.
            4. Never modify product prices.
            5. ProductId must exactly match one of the provided ProductId values.
            6. Recommend at most 3 products.
            7. Rank products from best to worst.
            8. Explain why each product matches the customer's requirements.
            9. If no product is suitable, return an empty recommendations array.
            10. Respond in Vietnamese.
            """,

                response_format = new
                {
                    type = "text",
                    mime_type = "application/json",
                    schema
                }
            };

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://generativelanguage.googleapis.com/v1beta/interactions");

            request.Headers.Add(
                "x-goog-api-key", _options.ApiKey
                );

            request.Content = JsonContent.Create(requestBody);

            using var response = await _httpClient.SendAsync(
                request,
                cancellationToken);

            var responseBody =
                await response.Content.ReadAsStringAsync(
                    cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Gemini API request failed. " +
                    $"StatusCode: {response.StatusCode}. " +
                    $"Response: {responseBody}");
            }

            var jsonText =
                ExtractTextFromInteraction(responseBody);

            var result =
                JsonSerializer.Deserialize<ProductRecommendationResponse>(
                    jsonText,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (result is null)
            {
                throw new InvalidOperationException(
                    "Failed to deserialize Gemini recommendation response.");
            }

            return result;
        }

        private static string ExtractTextFromInteraction(string responseBody)
        {
            using var json = JsonDocument.Parse(responseBody);

            if (!json.RootElement.TryGetProperty("steps", out var steps))
            {
                throw new InvalidOperationException(
                    "Gemini response does not contain 'steps'.");
            }

            foreach (var step in steps.EnumerateArray())
            {
                if (!step.TryGetProperty("type", out var type))
                {
                    continue;
                }

                if (type.GetString() != "model_output")
                {
                    continue;
                }

                if (!step.TryGetProperty("content", out var content))
                {
                    continue;
                }

                foreach (var item in content.EnumerateArray())
                {
                    if (!item.TryGetProperty("type", out var contentType))
                    {
                        continue;
                    }

                    if (contentType.GetString() != "text")
                    {
                        continue;
                    }

                    if (item.TryGetProperty("text", out var text))
                    {
                        return text.GetString() ?? string.Empty;
                    }
                }
            }

            throw new InvalidOperationException(
                "Gemini response does not contain text output.");
        }

        public async Task<AiResponse> SendMessageAsync(string message, string? previousInteractionId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_options.ApiKey))
            {
                throw new InvalidOperationException(
                    "Gemini API key is missing.");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException(
                    "Message cannot be empty.",
                    nameof(message));
            }

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://generativelanguage.googleapis.com/v1beta/interactions");

            request.Headers.Add(
                "x-goog-api-key",
                _options.ApiKey);

            var body = new Dictionary<string, object>
            {
                ["model"] = _options.Model,
                ["input"] = message
            };

            if (!string.IsNullOrWhiteSpace(previousInteractionId))
            {
                body["previous_interaction_id"] =
                    previousInteractionId;
            }

            request.Content = JsonContent.Create(body);

            using var response = await _httpClient.SendAsync(
                request,
                cancellationToken);

            var responseBody =
                await response.Content.ReadAsStringAsync(
                    cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Gemini API request failed. " +
                    $"StatusCode: {response.StatusCode}. " +
                    $"Response: {responseBody}");
            }

            using var json =
                JsonDocument.Parse(responseBody);

            var root = json.RootElement;

            // ==========================================
            // Interaction ID
            // ==========================================

            if (!json.RootElement.TryGetProperty("id", out var idElement))
            {
                throw new InvalidOperationException(
                    "Gemini response does not contain interaction id.");
            }

            var interactionId = idElement.GetString();

            if (string.IsNullOrWhiteSpace(interactionId))
            {
                throw new InvalidOperationException(
                    "Gemini returned an empty interaction id.");
            }

            // ==========================================
            // Token usage
            // ==========================================

            int? inputTokens = null;
            int? outputTokens = null;

            if (root.TryGetProperty(
                    "usage",
                    out var usage))
            {
                if (usage.TryGetProperty(
                        "total_input_tokens",
                        out var inputTokenElement))
                {
                    inputTokens = inputTokenElement.GetInt32();
                }

                if (usage.TryGetProperty(
                        "total_output_tokens",
                        out var outputTokenElement))
                {
                    outputTokens = outputTokenElement.GetInt32();
                }
            }

            // ==========================================
            // Model output
            // ==========================================

            if (!root.TryGetProperty(
                    "steps",
                    out var steps))
            {
                throw new InvalidOperationException(
                    "Gemini response does not contain 'steps'.");
            }

            foreach (var step in steps.EnumerateArray())
            {
                if (!step.TryGetProperty(
                        "type",
                        out var type))
                {
                    continue;
                }

                if (type.GetString() != "model_output")
                {
                    continue;
                }

                if (!step.TryGetProperty(
                        "content",
                        out var content))
                {
                    continue;
                }

                foreach (var item in content.EnumerateArray())
                {
                    if (!item.TryGetProperty(
                            "text",
                            out var text))
                    {
                        continue;
                    }

                    var contentText = text.GetString();

                    if (string.IsNullOrWhiteSpace(contentText))
                    {
                        throw new InvalidOperationException(
                            "Gemini returned empty text output.");
                    }

                    return new AiResponse
                    {
                        Content = contentText,
                        InteractionId = interactionId,
                        InputTokens = inputTokens,
                        OutputTokens = outputTokens
                    };
                }
            }

            throw new InvalidOperationException(
                "Gemini response does not contain model output.");
        }

        public async Task<TechStore.Model.DTOs.Ai.Ver2.AiResponseV2> GenerateResponseAsync(string message, AiConversationContext? conversationContext, CancellationToken cancellationToken = default)
        {
            var tools = new object[]
            {
                new
                {
                    type = "function",
                    name = "search_products",
                    description =
                        "Search TechStore products based on the customer's requirements. " +
                        "Use this function when the customer wants actual product recommendations.",

                    parameters = new
                    {
                        type = "object",

                        properties = new
                        {
                            isProductRelated = new
                            {
                                type = "boolean",
                                description = "Whether the request relates to TechStore products."
                            },

                            searchQuery = new
                            {
                                type = "string",
                                description = """
                                    The specific product name or model that the user wants to find.

                                    Rules:
                                    - Return only the product name or model, not the entire user message.
                                    - If the user asks for products based on category, price, brand,
                                      usage, or gaming needs without specifying a particular product,
                                      return an empty string "".
                                    - Do not include price constraints, category names, or usage
                                      descriptions unless they are part of the actual product name.
                                    - If the user mentions a specific product model, extract its name.

                                    Examples:
                                    - "Find iPhone 18 Pro Max" -> "iPhone 18 Pro Max"
                                    - "Find phones under 5 million VND" -> ""
                                    - "Find a Samsung phone under 10 million VND" -> ""
                                    - "Find a phone for gaming" -> ""
                                    - "Find a laptop for programming" -> ""
                                    - "Compare iPhone 18 Pro Max and Samsung Galaxy S26 Ultra"
                                      -> "iPhone 18 Pro Max; Samsung Galaxy S26 Ultra"
                                    """
                            },

                            category = new
                            {
                                type = "string",
                                description = "Product category, such as Phone or Laptop."
                            },

                            minPrice = new
                            {
                                type = "number",
                                description = "Minimum budget in VND, if specified."
                            },

                            maxPrice = new
                            {
                                type = "number",
                                description = "Maximum budget in VND, if specified."
                            },

                            brand = new
                            {
                                type = "string",
                                description = "Requested brand, if specified."
                            },

                            usages = new
                            {
                                type = "array",
                                items = new
                                {
                                    type = "string"
                                },
                                description = "Intended uses, such as programming or gaming."
                            },

                            games = new
                            {
                                type = "array",
                                items = new
                                {
                                    type = "string"
                                },
                                description = "Games the customer wants to play."
                            }
                        },

                        required = new[]
                        {
                            "isProductRelated",
                            "searchQuery",
                            "category",
                            "minPrice",
                            "maxPrice",
                            "brand",
                            "usages",
                            "games"
                        }
                    }
                }
            };

            var contextJson = conversationContext is null
                ? "{}"
                : JsonSerializer.Serialize(
                    conversationContext,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

            var requestBody = new
            {
                model = _options.Model,

                input = $"""
                    You are the AI Technology Assistant of TechStore.

                    SCOPE:
                    You may only assist with technology, electronic products,
                    product specifications, technology comparisons and purchasing advice.

                    You may answer general technology questions directly.
                    For example:
                    - Explain CPU, GPU, RAM, battery and camera technologies.
                    - Explain the differences between chipsets.
                    - Provide technology purchasing advice.

                    FUNCTION USAGE:
                    You have access to the search_products function.

                    Call search_products when the customer wants to:
                    - Find actual products available in TechStore.
                    - Receive product recommendations from TechStore.
                    - Find products matching specific requirements.
                    - Continue a previous product search that requires new database results.

                    Do NOT call search_products merely because the customer
                    mentions a product, chipset, battery, camera or other technology.

                    For general technology questions that do not require
                    TechStore product data, answer directly.

                    Do not invent TechStore products, prices, inventory or specifications.

                    CONVERSATION CONTEXT:
                    {contextJson}

                    CURRENT CUSTOMER MESSAGE:
                    {message}

                    CONTEXT RULES:
                    1. Understand the current message using the conversation context.
                    2. If the customer changes to a different product category,
                       do not carry over incompatible requirements from the previous category.
                    3. If the customer modifies a requirement within the same category,
                       preserve other relevant requirements.
                    4. Do not invent prices or product requirements.
                    5. Interpret Vietnamese currency values as VND.
                    6. When the customer explicitly requests actual product recommendations,
                       use search_products rather than inventing products.
                    7. If the request is outside the allowed technology scope,
                       politely explain that you can only help with technology-related topics.
                    """,

                tools 
            };

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://generativelanguage.googleapis.com/v1beta/interactions");

            request.Headers.Add(
                "x-goog-api-key",
                _options.ApiKey);

            request.Content = JsonContent.Create(requestBody);

            using var response = await _httpClient.SendAsync(
                request,
                cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Gemini API request failed. " +
                    $"StatusCode: {response.StatusCode}. " +
                    $"Response: {responseBody}");
            }

            // Parse the Interactions API response.
            // This helper must map the actual API response structure
            // to the AiResponse DTO.
            return ParseAiResponse(responseBody);
        }

        private TechStore.Model.DTOs.Ai.Ver2.AiResponseV2 ParseAiResponse(string responseBody)
        {
            using var document = JsonDocument.Parse(responseBody);

            var root = document.RootElement;

            var interactionId = root
                .GetProperty("id")
                .GetString();

            if (string.IsNullOrWhiteSpace(interactionId))
            {
                throw new InvalidOperationException(
                    "Gemini response does not contain a valid interaction ID.");
            }

            int? inputTokens = null;
            int? outputTokens = null;

            if (root.TryGetProperty("usage", out var usage))
            {
                if (usage.TryGetProperty("total_input_tokens", out var inputTokenElement))
                {
                    inputTokens = inputTokenElement.GetInt32();
                }

                if (usage.TryGetProperty("total_output_tokens", out var outputTokenElement))
                {
                    outputTokens = outputTokenElement.GetInt32();
                }
            }

            var result = new TechStore.Model.DTOs.Ai.Ver2.AiResponseV2() 
            { 
                InteractionId = interactionId, 
                InputTokens = inputTokens, 
                OutputTokens = outputTokens 
            };

            if (!root.TryGetProperty("steps", out var steps))
            {
                throw new InvalidOperationException(
                    "Gemini response does not contain steps.");
            }

            foreach (var step in steps.EnumerateArray())
            {
                if (!step.TryGetProperty("type", out var typeElement))
                {
                    continue;
                }

                var type = typeElement.GetString();

                switch (type)
                {
                    case "model_output":
                        {
                            if (!step.TryGetProperty("content", out var content))
                            {
                                continue;
                            }

                            foreach (var part in content.EnumerateArray())
                            {
                                if (part.TryGetProperty("type", out var partType)
                                    && partType.GetString() == "text"
                                    && part.TryGetProperty("text", out var textElement))
                                {
                                    var text = textElement.GetString();

                                    if (!string.IsNullOrWhiteSpace(text))
                                    {
                                        result.Text = string.IsNullOrWhiteSpace(result.Text)
                                            ? text
                                            : result.Text + Environment.NewLine + text;
                                    }
                                }
                            }

                            break;
                        }

                    case "function_call":
                        {
                            var name = step.TryGetProperty("name", out var nameElement)
                                ? nameElement.GetString()
                                : null;

                            if (string.IsNullOrWhiteSpace(name))
                            {
                                throw new InvalidOperationException(
                                    "Gemini returned a function call without a name.");
                            }

                            if (!step.TryGetProperty("arguments", out var arguments))
                            {
                                throw new InvalidOperationException(
                                    $"Gemini function '{name}' has no arguments.");
                            }

                            var callId = step.TryGetProperty("id", out var idElement)
                                ? idElement.GetString()
                                : null;

                            result.FunctionCalls.Add(new TechStore.Model.DTOs.Ai.Ver2.AiFunctionCall
                            {
                                Name = name,
                                CallId = callId ?? string.Empty,
                                Arguments = arguments.Clone()
                            });

                            break;
                        }
                }
            }

            if (result.FunctionCalls.Count == 0
                && string.IsNullOrWhiteSpace(result.Text))
            {
                throw new InvalidOperationException(
                    "Gemini returned neither text nor a function call.");
            }

            return result;
        }

        public async Task<AiResult> GenerateResponseFromToolResultsAsync(
            string message,
            AiConversationContext? conversationContext,
            AiResponseV2 previousResponse,
            List<TechStore.Model.DTOs.Ai.Ver2.AiToolResult> toolResults,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(previousResponse.InteractionId))
            {
                throw new ArgumentException(
                    "InteractionId is required.",
                    nameof(previousResponse));
            }

            if (toolResults.Count == 0)
            {
                throw new ArgumentException(
                    "At least one tool result is required.",
                    nameof(toolResults));
            }

            var functionCallsById = previousResponse.FunctionCalls
                .Where(x => !string.IsNullOrWhiteSpace(x.CallId))
                .ToDictionary(x => x.CallId);

            var functionResults = new List<object>();

            foreach (var toolResult in toolResults)
            {
                if (string.IsNullOrWhiteSpace(toolResult.CallId) ||
                    !functionCallsById.TryGetValue(
                        toolResult.CallId,
                        out var functionCall))
                {
                    throw new InvalidOperationException(
                        $"Unknown function call ID: {toolResult.CallId}");
                }

                if (!string.Equals(
                        functionCall.Name,
                        toolResult.FunctionName,
                        StringComparison.Ordinal))
                {
                    throw new InvalidOperationException(
                        "Function result does not match the original function call.");
                }

                var resultJson = toolResult.IsSuccess
                    ? JsonSerializer.Serialize(toolResult.Result)
                    : JsonSerializer.Serialize(new
                    {
                        error = toolResult.ErrorMessage
                            ?? "The requested operation failed."
                    });

                functionResults.Add(new
                {
                    type = "function_result",
                    name = toolResult.FunctionName,
                    call_id = toolResult.CallId,
                    result = new[]
                    {
                        new
                        {
                            type = "text",
                            text = resultJson
                        }
                    }
                });
            }

            var schema = new
            {
                type = "object",
                properties = new
                {
                    summary = new
                    {
                        type = "string"
                    },
                    recommendations = new
                    {
                        type = "array",

                        items = new
                        {
                            type = "object",

                            properties = new
                            {
                                productId = new
                                {
                                    type = "string",
                                    description =
                                        "The exact ProductId from the provided product list."
                                },

                                slug = new
                                {
                                    type = "string",
                                    description =
                                        "The exact Slug from the provided product list."
                                },

                                productName = new
                                {
                                    type = "string",
                                    description =
                                        "Name of the product."
                                },

                                imgUrl = new
                                {
                                    type = "string",
                                    description =
                                        "The URL of the product's main image."
                                },

                                price = new
                                {
                                    type = "number",
                                    description =
                                        "The price of the product."
                                },

                                rank = new
                                {
                                    type = "integer",
                                    description =
                                        "Recommendation ranking. 1 is the best."
                                },

                                reason = new
                                {
                                    type = "string",
                                    description =
                                        "Why this product is suitable for the customer."
                                }
                            },

                            required = new[]
                            {
                                "productId",
                                "slug",
                                "productName",
                                "imgUrl",
                                "price",
                                "rank",
                                "reason"
                            }
                        }
                    }
                },
                required = new[]
                {
                    "summary",
                    "recommendations"
                },
                additionalProperties = false
            };

            var instructions = """
                You are the AI shopping assistant of TechStore.
                Respond in Vietnamese.

                Return a JSON object matching the provided schema.

                Rules:
                1. Answer the customer's latest request using the conversation
                   and the function results.
                2. Recommend products only when relevant to the request.
                3. Use only ProductId values returned by successful
                   search_products function calls.
                4. Never invent product IDs or product specifications.
                5. Recommend at most 3 products.
                6. Rank recommendations from best to worst, starting at 1.
                7. Explain the reason for each recommendation in Vietnamese.
                8. If no suitable products are found, return an empty
                   recommendations array.
                9. Return only fields defined in the schema.
                10. Do not include Markdown fences or text outside the JSON.
                """;

            var requestBody = new
            {
                model = _options.Model,

                previous_interaction_id = previousResponse.InteractionId,

                system_instruction = instructions,

                input = functionResults,

                response_format = new
                {
                    type = "text",
                    mime_type = "application/json",
                    schema
                }
            };

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://generativelanguage.googleapis.com/v1beta/interactions");

            request.Headers.Add("x-goog-api-key", _options.ApiKey);
            request.Content = JsonContent.Create(requestBody);

            using var response = await _httpClient.SendAsync(
                request,
                cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Gemini API request failed. " +
                    $"StatusCode: {response.StatusCode}. " +
                    $"Response: {responseBody}");
            }

            // Parse response của Interactions API.
            var finalResponse = ParseAiResponse(responseBody);

            if (finalResponse.FunctionCalls.Count > 0)
            {
                throw new InvalidOperationException(
                    "Gemini requested additional function calls. " +
                    "The caller must execute them before returning the final answer.");
            }

            if (string.IsNullOrWhiteSpace(finalResponse.Text))
            {
                throw new InvalidOperationException(
                    "Gemini returned an empty final response.");
            }

            using var jsonDocument = JsonDocument.Parse(finalResponse.Text);

            var recommendation = JsonSerializer.Deserialize<ProductRecommendationResponse>(
                finalResponse.Text,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (recommendation is null)
            {
                throw new InvalidOperationException(
                    "Failed to deserialize Gemini recommendation response.");
            }

            // Thu thập các sản phẩm hợp lệ từ kết quả search_products.
            var availableProducts = toolResults
                .Where(x =>
                    x.IsSuccess &&
                    string.Equals(
                        x.FunctionName,
                        "search_products",
                        StringComparison.Ordinal))
                .SelectMany(x => ExtractProducts(x.Result))
                .GroupBy(x => x.ProductId, StringComparer.Ordinal)
                .ToDictionary(
                    g => g.Key,
                    g => g.First(),
                    StringComparer.Ordinal);

            // Chỉ giữ ProductId tồn tại trong dữ liệu backend.
            var validatedRecommendations = recommendation.Recommendations
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.ProductId) &&
                    availableProducts.ContainsKey(x.ProductId))
                .GroupBy(x => x.ProductId, StringComparer.Ordinal)
                .Select(g => g.OrderBy(x => x.Rank).First())
                .OrderBy(x => x.Rank)
                .Take(3)
                .Select((item, index) =>
                {
                    var product = availableProducts[item.ProductId];

                    return new ProductRecommendation
                    {
                        ProductId = product.ProductId,
                        Slug = product.Slug,
                        ImgUrl = product.ImgUrl,
                        ProductName = product.Name,
                        Price = product.Price,
                        Rank = index + 1,
                        Reason = item.Reason
                    };
                })
                .ToList();

            recommendation.Recommendations = validatedRecommendations;

            // Đọc token usage từ response gốc.
            int? inputTokens = null;
            int? outputTokens = null;

            using var responseDocument = JsonDocument.Parse(responseBody);
            var root = responseDocument.RootElement;

            if (root.TryGetProperty("usage", out var usage))
            {
                if (usage.TryGetProperty(
                        "total_input_tokens",
                        out var inputTokenElement) &&
                    inputTokenElement.TryGetInt32(out var inputTokenCount))
                {
                    inputTokens = inputTokenCount;
                }

                if (usage.TryGetProperty(
                        "total_output_tokens",
                        out var outputTokenElement) &&
                    outputTokenElement.TryGetInt32(out var outputTokenCount))
                {
                    outputTokens = outputTokenCount;
                }
            }

            return new AiResult
            {
                InteractionId = finalResponse.InteractionId
                    ?? throw new InvalidOperationException(
                        "Gemini response does not contain an interaction ID."),
                InputTokens = inputTokens,
                OutputTokens = outputTokens,
                Recommendation = recommendation
            };
        }

        private static IEnumerable<AiProductContext> ExtractProducts(object? result)
        {
            if (result is IEnumerable<AiProductContext> products)
            {
                return products;
            }

            if (result is AiProductContext product)
            {
                return [product];
            }

            return [];
        }
    }
}