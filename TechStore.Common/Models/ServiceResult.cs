using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Common.Models
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public string? ErrorCode { get; set; }

        public static ServiceResult<T> Success(T data, string? message = null)
            => new() { IsSuccess = true, Data = data, Message = message };

        public static ServiceResult<T> Fail(string errorCode, string message)
            => new() { IsSuccess = false, ErrorCode = errorCode, Message = message };
    }
}
