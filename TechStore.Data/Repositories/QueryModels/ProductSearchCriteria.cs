using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Repositories.QueryModels
{
    public class ProductSearchCriteria
    {
        public bool IsProductRelated { get; set; }
        public string? Category { get; set; }
        public string? Brand { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public List<string> Usages { get; set; } = [];

        public List<string> Games { get; set; } = [];
    }
}
