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
        public static AdminListItemProductStatisticModel ToAdminListItemProductStatisticModel(this Product product)
        {
            return new AdminListItemProductStatisticModel
            {
                ProductVariantOptionId = product.PublicId,
                Name = product.Name,
                MainImageUrl = product.MainImageUrl,
                CategoryName = "",
                Stock = 0,
                Price = 0,

                AverageRating = product.AverageRating,
                RatedCount = product.RatedCount,
                SoldCount = product.SoldCount,
                StartSellingDate = product.StartSellingDate
            };
        }

        public static AdminListItemProduct ToAdminProductListItem(this Product product)
        {
            return new AdminListItemProduct
            {
                Id = product.PublicId,
                Name = product.Name,
                MainImageUrl = product.MainImageUrl,
                SoldCount = product.SoldCount,

                AverageRating = product.AverageRating,
                RatedCount = product.RatedCount,
                StartSellingDate = product.StartSellingDate
            };
        }

        public static ListItemProductModel ToProductListItem(this Product product)
        {
            return new ListItemProductModel
            {
                id = product.PublicId,
                ProductVariantId = product.PublicId,
                ProductName = product.Name,
                ProductVariantName = product.Name,
                Slug = product.Slug,
                Warranty = product.Warranty,

                CategoryName = product.Category?.Name,
                MainImageUrl = product.MainImageUrl,

                Price = product.MinPrice,
                SalePrice = product.SalePrice,

                AverageRating = product.AverageRating,
                RatedCount = product.RatedCount,
                SoldCount = product.SoldCount,
                StartSellingDate = product.StartSellingDate
            };
        }

        public static List<ListItemProductModel> ToListProductListItem(this List<Product> products)
        {
            var list = new List<ListItemProductModel>();

            foreach (var item in products)
            {
                list.Add(ToProductListItem(item));
            }

            return list;
        }

        public static ListItemProductModel VariantToProductListItem(this ProductVariant productVariant)
        {
            return new ListItemProductModel
            {
                id = productVariant.Product.PublicId,
                ProductVariantId = productVariant.PublicId,
                ProductName = productVariant.Product.Name,
                ProductVariantName = productVariant.Name,
                Slug = productVariant.Product.Slug,
                Warranty = productVariant.Product.Warranty,

                CategoryName = productVariant.Product.Category?.Name,
                MainImageUrl = productVariant.Product.MainImageUrl,

                Price = productVariant.Price,
                SalePrice = 0,

                AverageRating = productVariant.Product.AverageRating,
                RatedCount = productVariant.Product.RatedCount,
                SoldCount = productVariant.SoldCount,

                StartSellingDate = productVariant.Product.StartSellingDate
            };
        }

        public static List<ListItemProductModel> VariantsToListProductListItem(this List<ProductVariant> productVariants)
        {
            var list = new List<ListItemProductModel>();

            foreach (var item in productVariants)
            {
                list.Add(VariantToProductListItem(item));
            }

            return list;
        }

        public static ProductDetailModel ToProductDetail(this Product product)
        {
            return new ProductDetailModel
            {
                Id = product.PublicId,
                Name = product.Name,
                ShortDescription = product.ShortDescription,

                Description = product.Description,
                MainImageUrl = product.MainImageUrl,
                GalleryImageUrls = product.GalleryImageUrls,

                SoldCount = product.SoldCount,
                Warranty = product.Warranty,

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
                PublishDate = product.PublishDate,

                Variants = product.Variants.Select(v => v.ToProductVariantResponseModel()).ToList()
            };
        }

        public static AdminProductDetailModel ToAdminProductDetail(this Product product, Category category, Brand brand)
        {
            return new AdminProductDetailModel
            {
                Id = product.PublicId,
                Name = product.Name,
                ShortDescription = product.ShortDescription,

                BrandId = product.BrandPublicId,
                CategoryId = category.PublicId,

                Description = product.Description,
                MainImageUrl = product.MainImageUrl,
                GalleryImageUrls = product.GalleryImageUrls,

                SoldCount = product.SoldCount,

                SalePrice = product.SalePrice,
                SaleStart = product.SaleStart,
                SaleEnd = product.SaleEnd,

                StartSellingDate = product.StartSellingDate,
                EndSellingDate = product.EndSellingDate,

                //Category = category.ToCategoryResponseModel(),

                //Brand = brand.ToBrandResponseModel(),

                AverageRating = product.AverageRating,
                RatedCount = product.RatedCount,

                Slug = product.Slug,
                Tags = product.Tags,
                Warranty = product.Warranty,
                CreatedAt = product.CreatedAt,

                IsOnSale = product.IsOnSale,
                IsFeatured = product.IsFeatured,
                EntityStatus = product.EntityStatus,

                PublishDate = product.PublishDate,

                Variants = product.Variants.Select(v => v.ToAdminProductVariantResponseModel()).ToList()
            };
        }

        public static ProductVariantDetailModel ToProductVariantResponseModel(this ProductVariant variant)
        {
            return new ProductVariantDetailModel
            {
                Id = variant.PublicId,
                ProductId = variant.Product.PublicId,
                Name = variant.Name,
                Description = variant.Description,
                Price = variant.Price,
                SoldCount = variant.SoldCount,
                Options = variant.Options.Select(o => o.ToProductVariantOptionResponseModel()).ToList(),
            };
        }

        public static AdminProductVariantDetailModel ToAdminProductVariantResponseModel(this ProductVariant variant)
        {
            return new AdminProductVariantDetailModel
            {
                Id = variant.PublicId,
                ProductId = variant.Product.PublicId,
                Name = variant.Name,
                Description = variant.Description,
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
                Id = option.PublicId,
                ProductVariantId = option.ProductVariant.PublicId,
                Name = option.Name,
                ImageUrl = option.ImageUrl,
                Stock = option.Stock,
                Price = option.Price,
                ImportPrice = option.ImportPrice,
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
