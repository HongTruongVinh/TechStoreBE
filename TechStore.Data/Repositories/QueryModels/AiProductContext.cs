using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Repositories.QueryModels
{
    public class AiProductContext
    {
        public required string ProductId { get; set; }

        public required string Slug { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        public required string ImgUrl { get; set; }

        public decimal Price { get; set; }

        public string? Cpu { get; set; }

        public string? Gpu { get; set; }

        public int? Ram { get; set; }

        public int? Storage { get; set; }
    }
}
