using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Product;
using TechStore.Model.DTOs.ProductVariant;
using TechStore.Model.DTOs.ProductVariantOption;

namespace TechStore.Service.Interfaces
{
    public interface IProductService
    {

        // 2. Lọc sản phẩm (theo category, price, brand, sort, v.v.)
        Task<ServiceResult<List<ProductListItemModel>>> GetProductsFilteredAsync(
            int pageNumber,
            int pageSize,
            string? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? brandId = null
        );

        Task<ServiceResult<List<ProductListItemModel>>> GetProductsAsync(int page, int pageSize);
        Task<ServiceResult<List<ProductListItemModel>>> SearchByNameAsync(string keyword, int page, int pageSize);
        Task<ServiceResult<List<ProductListItemModel>>> GetProductsByCategory(string categorySlug, int page, int pageSize);
        Task<ServiceResult<List<ProductListItemModel>>> GetProductsByCategoryAndBrand(string categorySlug, string brandSlug, int page, int pageSize);
        Task<ServiceResult<ProductDetailModel>> GetProductById(string publicId);

        Task<ServiceResult<bool>> UpdateProductInformation(string publicId, ProductUpdateModel model);
        Task<ServiceResult<bool>> UpdateProductCount(string publicId, ProductCountUpdateModel model);
        Task<ServiceResult<bool>> DeleteProduct(string publicId);
        Task<ServiceResult<string>> AddProduct(ProductCreateModel model);
        Task<ServiceResult<AdminProductDetailModel>> GetAdminProductById(string publicId);
        Task<ServiceResult<List<AdminProductDetailModel>>> GetAdminProducts(int pageNumber, int pageSize);

        Task<ServiceResult<List<ProductListItemModel>>> GetUserRecommendedProducts(string userId);
        Task<ServiceResult<List<ProductListItemModel>>> GetHotProducts();
        Task<ServiceResult<List<ProductListItemModel>>> GetFeaturedProducts();

        Task<ServiceResult<string>> AddProductVariantAsync(string productId, ProductVariantCreateModel model);
        Task<ServiceResult<bool>> UpdateProductVariantAsync(string productId, string variantId, ProductVariantUpdateModel model);
        Task<ServiceResult<bool>> DeleteProductVariantAsync(string productId, string variantId);
        Task<ServiceResult<string>> AddProductVariantOptionAsync(string productId, string variantId, ProductVariantOptionCreateModel model);
        Task<ServiceResult<bool>> UpdateProductVariantOptionAsync(string productId, string variantId, string optionId, ProductVariantOptionUpdateModel model);
        Task<ServiceResult<bool>> DeleteProductVariantOptionAsync(string productId, string variantId, string optionId);

    }

    //public interface IProductService
    //{

    //    // 2. Lọc sản phẩm (theo category, price, brand, sort, v.v.)
    //    Task<ServiceResult<List<ProductListItemModel>>> GetProductsFilteredAsync(
    //        int pageNumber,
    //        int pageSize,
    //        string? categoryId = null,
    //        decimal? minPrice = null,
    //        decimal? maxPrice = null,
    //        string? brandId = null
    //    );

    //    Task<ServiceResult<List<ProductListItemModel>>> GetProductsAsync(int page, int pageSize);
    //    Task<ServiceResult<List<ProductListItemModel>>> SearchByNameAsync(string keyword, int page, int pageSize);
    //    Task<ServiceResult<ProductDetailModel>> GetProductById(string id);

    //    Task<ServiceResult<bool>> UpdateProductInformation(string id, ProductUpdateModel model);
    //    Task<ServiceResult<bool>> UpdateProductCount(string id, ProductCountUpdateModel model);
    //    Task<ServiceResult<bool>> DeleteProduct(string id);
    //    Task<ServiceResult<string>> AddProduct(ProductCreateModel model);
    //    Task<string> SeedDataProduct(ProductCreateModel model);
    //    Task<ServiceResult<AdminProductDetailModel>> GetAdminProductById(string id);
    //    Task<ServiceResult<List<AdminProductDetailModel>>> GetAdminProducts(int pageNumber, int pageSize);

    //    Task<ServiceResult<List<ProductListItemModel>>> GetUserRecommendedProducts(string userId);
    //    Task<ServiceResult<List<ProductListItemModel>>> GetHotProducts();
    //    Task<ServiceResult<List<ProductListItemModel>>> GetFeaturedProducts();
    //}
}
