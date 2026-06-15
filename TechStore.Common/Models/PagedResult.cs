using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Common.Models
{
    public class PagedResult<T>
    {
        public required int CurrentPage { get; init; }
        public required int PageSize { get; init; }
        public required int TotalItems { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalItems / PageSize);

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public List<T> Items { get; set; } = new List<T>();
    }
}
