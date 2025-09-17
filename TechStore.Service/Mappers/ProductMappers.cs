using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.Product;
using TechStore.Model.DTOs.ProductVariant;
using TechStore.Model.DTOs.ProductVariantOption;

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
                SalePrice = product.SalePrice,
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

        public static ProductDetailModel ToProductDetail(this Product product)
        {
            return new ProductDetailModel
            {
                ProductId = product.PublicId,
                Name = product.Name,
                ShortDescription = product.ShortDescription,

                Description = product.Description,
                MainImageUrl = product.MainImageUrl,
                GalleryImageUrls = product.GalleryImageUrls ?? new List<string>(),

                SoldCount = product.SoldCount,

                SalePrice = product.SalePrice,
                SaleStart = product.SaleStart,
                SaleEnd = product.SaleEnd,

                CategoryId = product.Category.PublicId,
                CategoryName = product.Category.Name,

                BrandId = product.PublicId,
                BrandName = product.Brand.Name,

                AverageRating = product.AverageRating,
                RatedCount = product.RatedCount,

                Slug = product.Slug,
                Tags = product.Tags,

                IsOnSale = product.IsOnSale,
                IsFeatured = product.IsFeatured,

                Variants = product.Variants.Select(v => v.ToProductVariantResponseModel()).ToList()
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

                SoldCount = product.SoldCount,

                SalePrice = product.SalePrice,
                SaleStart = product.SaleStart,
                SaleEnd = product.SaleEnd,

                StartSellingDate = product.StartSellingDate,
                EndSellingDate = product.EndSellingDate,

                Category = category.ToCategoryResponseModel(),

                Brand = brand.ToBrandResponseModel(),

                AverageRating = product.AverageRating,
                RatedCount = product.RatedCount,

                Slug = product.Slug,
                Tag = product.Tags,
                CreatedAt = product.CreatedAt,

                IsOnSale = product.IsOnSale,
                IsFeatured = product.IsFeatured,
                EntityStatus = product.EntityStatus,

                Variants = product.Variants.Select(v => v.ToAdminProductVariantResponseModel()).ToList()
            };
        }

        public static ProductVariantDetailModel ToProductVariantResponseModel(this ProductVariant variant)
        {
            return new ProductVariantDetailModel
            {
                ProductId = variant.Product.PublicId,
                Name = variant.Name,
                Price = variant.Price,
                SoldCount = variant.SoldCount,
                Options = variant.Options.Select(o => o.ToProductVariantOptionResponseModel()).ToList(),
            };
        }

        public static AdminProductVariantDetailModel ToAdminProductVariantResponseModel(this ProductVariant variant)
        {
            return new AdminProductVariantDetailModel
            {
                ProductId = variant.Product.PublicId,
                Name = variant.Name,
                Price = variant.Price,
                ImportPrice = variant.ImportPrice,
                SoldCount = variant.SoldCount,
                Options = variant.Options.Select(o => o.ToProductVariantOptionResponseModel()).ToList(),
            };
        }

        public static ProductVariantOptionDetailModel ToProductVariantOptionResponseModel(this ProductVariantOption option)
        {
            return new ProductVariantOptionDetailModel
            {
                ProductVariantId = option.ProductVariant.PublicId,
                Name = option.Name,
                ImageUrl = option.ImageUrl,
                Stock = option.Stock,
                Price = option.Price
            };
        }
    }

    //public static class ProductMappers
    //{
    //    public static AdminProductListItemModel ToAdminProductListItem(this Product product)
    //    {
    //        return new AdminProductListItemModel
    //        {
    //            ProductId = product.PublicId,
    //            Name = product.Name,
    //            MainImageUrl = product.MainImageUrl,
    //            Price = product.Price,
    //            Stock = product.Stock,
    //            AverageRating = product.AverageRating,
    //            RatedCount = product.RatedCount,
    //            SoldCount = product.SoldCount,
    //            StartSellingDate = product.StartSellingDate
    //        };
    //    }

    //    public static ProductListItemModel ToProductListItem(this Product product)
    //    {
    //        return new ProductListItemModel
    //        {
    //            ProductId = product.PublicId,
    //            Name = product.Name,
    //            CategoryName = product.Category?.Name,
    //            ShortDescription = product.ShortDescription,
    //            MainImageUrl = product.MainImageUrl,
    //            Price = product.Price,
    //            SalePrice = product.SalePrice,
    //            Stock = product.Stock,
    //            AverageRating = product.AverageRating,
    //            RatedCount = product.RatedCount,
    //            SoldCount = product.SoldCount,
    //            StartSellingDate = product.StartSellingDate
    //        };
    //    }

    //    public static List<ProductListItemModel> ToListProductListItem(this List<Product> product)
    //    {
    //        var list = new List<ProductListItemModel>();

    //        foreach (var item in product) 
    //        {
    //            list.Add(ToProductListItem(item));
    //        }

    //        return list;
    //    }

    //    public static ProductDetailModel ToProductDetail(this Product product, Category category, Brand brand)
    //    {
    //        return new ProductDetailModel
    //        {
    //            ProductId = product.PublicId,
    //            Name = product.Name,
    //            ShortDescription = product.ShortDescription,

    //            Description = product.Description,
    //            MainImageUrl = product.MainImageUrl,
    //            GalleryImageUrls = product.GalleryImageUrls ?? new List<string>(),

    //            Price = product.Price,
    //            Stock = product.Stock,
    //            SoldCount = product.SoldCount,

    //            SalePrice = product.SalePrice,
    //            SaleStart = product.SaleStart,
    //            SaleEnd = product.SaleEnd,

    //            CategoryId = product.Category.PublicId,
    //            CategoryName = category.Name,

    //            BrandId = product.PublicId,
    //            BrandName = brand.Name,

    //            AverageRating = product.AverageRating,
    //            RatedCount = product.RatedCount,

    //            Slug = product.Slug,
    //            Tags = product.Tags,

    //            IsOnSale = product.IsOnSale,
    //            IsFeatured = product.IsFeatured
    //        };
    //    }

    //    public static AdminProductDetailModel ToAdminProductDetail(this Product product, Category category, Brand brand)
    //    {
    //        return new AdminProductDetailModel
    //        {
    //            ProductId = product.PublicId,
    //            Name = product.Name,
    //            ShortDescription = product.ShortDescription,

    //            Description = product.Description,
    //            MainImageUrl = product.MainImageUrl,
    //            GalleryImageUrls = product.GalleryImageUrls ?? new List<string>(),

    //            Price = product.Price,
    //            Stock = product.Stock,
    //            SoldCount = product.SoldCount,

    //            SalePrice = product.SalePrice,
    //            SaleStart = product.SaleStart,
    //            SaleEnd = product.SaleEnd,

    //            StartSellingDate = product.StartSellingDate,
    //            EndSellingDate = product.EndSellingDate,

    //            Category = category.ToCategoryResponseModel(),

    //            Brand = brand.ToBrandResponseModel(),

    //            AverageRating = product.AverageRating,
    //            RatedCount = product.RatedCount,

    //            Slug = product.Slug,
    //            Tag = product.Tags,
    //            ImportPrice = product.ImportPrice,
    //            CreatedAt = product.CreatedAt,

    //            IsOnSale = product.IsOnSale,
    //            IsFeatured = product.IsFeatured,
    //            EntityStatus = product.EntityStatus
    //        };
    //    }
    //}
}
