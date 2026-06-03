using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Common.Models
{
    public class PagedResult<T>
    {
        public int CurrentPage { get; init; }
        public int PageSize { get; init; }
        public int TotalItems { get; init; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalItems / PageSize);

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public List<T> Items { get; set; } = new List<T>();
    }
}
