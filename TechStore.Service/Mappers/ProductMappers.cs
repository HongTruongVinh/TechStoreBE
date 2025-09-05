using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.Product;

namespace TechStore.Service.Mappers
{
    public static class ProductMappers
    {
        public static AdminProductListItemModel ToAdminProductListItem(this Product product)
        {
            return new AdminProductListItemModel
            {
                ProductId = product.PublicId,
                Name = product.Name,
                MainImageUrl = product.MainImageUrl,
                Price = product.Price,
                Stock = product.Stock,
                AverageRating = product.AverageRating,
                RatedCount = product.RatedCount,
                SoldCount = product.SoldCount,
                StartSellingDate = product.StartSellingDate
            };
        }

        public static ProductListItemModel ToProductListItem(this Product product)
        {
            return new ProductListItemModel
            {
                ProductId = product.PublicId,
                Name = product.Name,
                CategoryName = product.Category?.Name,
                ShortDescription = product.ShortDescription,
                MainImageUrl = product.MainImageUrl,
                Price = product.Price,
                Stock = product.Stock,
                AverageRating = product.AverageRating,
                RatedCount = product.RatedCount,
                SoldCount = product.SoldCount,
                StartSellingDate = product.StartSellingDate
            };
        }

        public static List<ProductListItemModel> ToListProductListItem(this List<Product> product)
        {
            var list = new List<ProductListItemModel>();

            foreach (var item in product) 
            {
                list.Add(ToProductListItem(item));
            }

            return list;
        }

        public static ProductDetailModel ToProductDetail(this Product product, Category category, Brand brand)
        {
            return new ProductDetailModel
            {
                ProductId = product.PublicId,
                Name = product.Name,
                ShortDescription = product.ShortDescription,

                Description = product.Description,
                MainImageUrl = product.MainImageUrl,
                GalleryImageUrls = product.GalleryImageUrls ?? new List<string>(),

                Price = product.Price,
                Stock = product.Stock,
                SoldCount = product.SoldCount,

                SalePrice = product.ImportPrice,
                SaleStart = product.SaleStart,
                SaleEnd = product.SaleEnd,

                CategoryId = product.Category.PublicId,
                CategoryName = category.Name,

                BrandId = product.PublicId,
                BrandName = brand.Name,

                AverageRating = product.AverageRating,
                RatedCount = product.RatedCount,

                IsOnSale = product.IsOnSale,
                IsFeatured = product.IsFeatured
            };
        }

        public static AdminProductDetailModel ToAdminProductDetail(this Product product, Category category, Brand brand)
        {
            return new AdminProductDetailModel
            {
                ProductId = product.PublicId,
                Name = product.Name,
                ShortDescription = product.ShortDescription,

                Description = product.Description,
                MainImageUrl = product.MainImageUrl,
                GalleryImageUrls = product.GalleryImageUrls ?? new List<string>(),

                Price = product.Price,
                Stock = product.Stock,
                SoldCount = product.SoldCount,

                SalePrice = product.ImportPrice,
                SaleStart = product.SaleStart,
                SaleEnd = product.SaleEnd,

                StartSellingDate = product.StartSellingDate,
                EndSellingDate = product.EndSellingDate,

                Category = category.ToCategoryResponseModel(),

                Brand = brand.ToBrandResponseModel(),

                AverageRating = product.AverageRating,
                RatedCount = product.RatedCount,

                Slug = product.Slug,
                Tag = product.Tag,
                ImportPrice = product.ImportPrice,
                CreatedAt = product.CreatedAt,

                IsOnSale = product.IsOnSale,
                IsFeatured = product.IsFeatured,
                EntityStatus = product.EntityStatus
            };
        }
    }
}
