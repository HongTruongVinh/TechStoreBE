using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Model.DTOs.RequestModel
{
    public class ListOrdersRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public EOrderStatus? Status { get; set; }  // Pending, Processing, Completed, Cancelled

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerId { get; set; }

        public string? SortBy { get; set; } = "CreatedDate"; // CreatedDate, TotalAmount
        public bool IsDescending { get; set; } = true;
    }
}
