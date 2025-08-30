using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Model.DTOs.Product;

namespace TechStore.Model.DTOs.Home
{
    public class HomeResponseModel
    {
        public required List<string> Banner { get; set; }
        public required List<ProductListItemModel> HotProducts { get; set; }
        public required List<ProductListItemModel> NewProducts { get; set; }
        public required List<ProductListItemModel> FeatureProducts { get; set; }
    }
}
