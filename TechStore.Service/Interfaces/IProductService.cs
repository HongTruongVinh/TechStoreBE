using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Category;
using TechStore.Model.DTOs.Product;
using TechStoreModel.Model.ResponseModel;

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
        Task<ServiceResult<ProductDetailModel>> GetProductById(string id);

        Task<ServiceResult<bool>> UpdateProductInformation(string id, ProductUpdateModel model);
        Task<ServiceResult<bool>> UpdateProductCount(string id, ProductCountUpdateModel model);
        Task<ServiceResult<bool>> DeleteProduct(string id);
        Task<ServiceResult<string>> AddProduct(ProductCreateModel model);
        Task<string> SeedDataProduct(ProductCreateModel model);
        Task<ServiceResult<AdminProductDetailModel>> GetAdminProductById(string id);
        Task<ServiceResult<List<AdminProductDetailModel>>> GetAdminProducts(int pageNumber, int pageSize);

        Task<ServiceResult<List<ProductListItemModel>>> GetUserRecommendedProducts(string userId);
        Task<ServiceResult<List<ProductListItemModel>>> GetHotProducts();
    }
}
