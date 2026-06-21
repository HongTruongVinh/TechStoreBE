using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Product
{
    public class AdminListItemProduct
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string MainImageUrl { get; set; }

        public required int SoldCount { get; set; }

        public required long AverageRating { get; set; }
        public required int RatedCount { get; set; }

        public DateTime? StartSellingDate { get; set; }
    }
}
