using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Enums;
using TechStore.Common.Models;

namespace TechStoreAPI.Extensions
{
    public static class ServiceResultExtensions
    {
        public static ActionResult<ApiResponse<T>> ToActionResult<T>(this ServiceResult<T> result, ControllerBase controller)
        {
            var response = new ApiResponse<T>
            {
                Success = result.IsSuccess,
                Message = result.Message ?? string.Empty,
                Data = result.Data
            };

            if (result.IsSuccess)
                return controller.Ok(response);

            return result.ErrorType switch
            {
                EErrorType.BadRequest => controller.BadRequest(response),

                EErrorType.NotFound => controller.NotFound(response),

                EErrorType.ConfictData => controller.Conflict(response),

                EErrorType.Unauthorized => controller.Unauthorized(response),

                EErrorType.Forbidden => controller.Forbid(),

                EErrorType.Status500InternalServerError => controller.StatusCode(500, response),

                _ => controller.StatusCode(500, response)
            };
        }
    }
}
