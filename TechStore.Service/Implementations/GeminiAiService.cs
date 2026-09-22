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
    }
}