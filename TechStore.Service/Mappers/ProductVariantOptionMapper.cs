using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.Product;

namespace TechStore.Service.Mappers
{
    public static class ProductVariantOptionMapper
    {
        public static AdminListItemProductStatisticModel ToAdminListItemModel(this ProductVariantOption pvo)
        {
            return new AdminListItemProductStatisticModel()
            {
                ProductVariantOptionId = pvo.PublicId,
                CategoryName = pvo.ProductVariant.Product.Category.Name,
                Name = pvo.ProductVariant.Product.Name + " " + pvo.ProductVariant.Name + " " + pvo.Name,
                SoldCount = pvo.SoldCount,
                Price = pvo.Price,
                MainImageUrl = pvo.ImageUrl,
                AverageRating = pvo.ProductVariant.Product.AverageRating,
                Stock = pvo.Stock,
                RatedCount = pvo.ProductVariant.Product.RatedCount,
            };
        }
    }
}
