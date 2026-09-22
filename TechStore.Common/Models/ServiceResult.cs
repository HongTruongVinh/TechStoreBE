using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Enums;

namespace TechStore.Common.Models
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }

        public EErrorType? ErrorType { get; set; }

        public static ServiceResult<T> Success(T? data = default, string message = Messenger.SuccessFull)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                ErrorType = null,
                Message = message,
                Data = data
            };
        }

        public static ServiceResult<T> Failure(EErrorType errorType, string message)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                ErrorType = errorType,
                Message = message
            };
        }
    }
}
