using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Brand
{
    public class BrandResponseModel
    {
        public required string BrandId { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public required string Slug { get; set; }

        public required string IconImageUrl { get; set; }
    }
}
