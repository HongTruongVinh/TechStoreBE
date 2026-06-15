using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Common.Models
{
    public class PagedQuery
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public bool Descending { get; set; }
    }



    public class ProductSearchQuery : PagedQuery
    {
        public string? Keyword { get; set; }
        public string? CategoryId { get; set; }
        public string? BrandId { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsActive { get; set; }
    }

    public class OrderSearchQuery : PagedQuery
    {
        public string? Keyword { get; set; }
        public string? UserId { get; set; }
        public EOrderStatus? OrderStatus { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
