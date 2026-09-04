using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Data.Repositories.QueryModels;
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
                            return ServiceResult<AiResponse>.Fail(EErrorType.Status500InternalServerError, string.Empty);
                        }

                        var chatResponse = new AiResponse { Content = text.GetString()! };

                        return ServiceResult<AiResponse>.Success(chatResponse);
                    }
                }
            }

            return ServiceResult<AiResponse>.Fail(EErrorType.Status500InternalServerError, string.Empty);
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
                return ServiceResult<AiResponse>.Fail(EErrorType.Status500InternalServerError, string.Empty);
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
                            return ServiceResult<AiResponse>.Fail(EErrorType.Status500InternalServerError, string.Empty);
                        }

                        var chatResponse = new AiResponse { Content = text.GetString()! };

                        return ServiceResult<AiResponse>.Success(chatResponse);
                    }
                }
            }

            return ServiceResult<AiResponse>.Fail(EErrorType.Status500InternalServerError, string.Empty);
        }

        public async Task<ProductSearchCriteria> ExtractProductSearchCriteriaAsync(string message, CancellationToken cancellationToken = default)
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

        public async Task<ProductRecommendationResponse> GenerateProductRecommendationAsync(string userQuery, IEnumerable<AiProductContext> products, CancellationToken cancellationToken = default)
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
    }
}