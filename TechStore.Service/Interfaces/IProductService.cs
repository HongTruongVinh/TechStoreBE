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

        #region Client Services
        Task<ServiceResult<PagedResult<ListItemProductModel>>> GetProductsAsync(ProductSearchQuery query);
        Task<ServiceResult<ProductDetailModel>> GetProductById(string publicId);

        Task<ServiceResult<List<ListItemProductModel>>> GetUserRecommendedProducts(string userId);
        Task<ServiceResult<List<ListItemProductModel>>> GetHotProducts();
        Task<ServiceResult<List<ListItemProductModel>>> GetFeaturedProducts();
        #endregion

        #region Admin Services
        Task<ServiceResult<AdminProductDetailModel>> GetAdminProductByIdAsync(string publicId);
        Task<ServiceResult<PagedResult<AdminListItemProduct>>> GetAdminProductsAsync(ProductSearchQuery query);

        Task<ServiceResult<string>> AddProduct(ProductCreateModel model);
        Task<ServiceResult<bool>> UpdateProduct(string publicId, ProductUpdateModel model);
        Task<ServiceResult<bool>> DeleteProduct(string publicId);

        Task<ServiceResult<string>> AddProductVariantAsync(string productId, ProductVariantCreateModel model);
        Task<ServiceResult<bool>> UpdateProductVariantAsync(string productId, string variantId, ProductVariantUpdateModel model);
        Task<ServiceResult<bool>> DeleteProductVariantAsync(string productId, string variantId);

        Task<ServiceResult<string>> AddProductVariantOptionAsync(string productId, string variantId, ProductVariantOptionCreateModel model);
        Task<ServiceResult<bool>> UpdateProductVariantOptionAsync(string productId, string variantId, string optionId, ProductVariantOptionUpdateModel model);
        Task<ServiceResult<bool>> DeleteProductVariantOptionAsync(string productId, string variantId, string optionId);

        Task<ServiceResult<bool>> UpdateProductCount(string publicId, ProductCountUpdateModel model);
        #endregion
    }
}
