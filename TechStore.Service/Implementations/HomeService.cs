using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Data.Repositories.Interfaces;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Home;
using TechStore.Model.DTOs.Product;
using TechStore.Service.Interfaces;
using TechStore.Service.Mappers;

namespace TechStore.Service.Implementations
{
    public class HomeService : IHomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;

        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        public HomeService(
            IUnitOfWork unitOfWork,
            IOrderService orderService,
            IProductService productService,
            IProductRepository productRepository
            )
        {
            _orderService = orderService;
            _productService = productService;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<HomeResponseModel>> GetHomeProduct()
        {
            var serviceResult = new ServiceResult<HomeResponseModel>
            {
                IsSuccess = true,
                Data = null,
                Message = Messenger.GetDataSuccessful
            };

            var homeData = new HomeResponseModel
            {
                Banner = new List<string>(),
                HotProducts = new List<ProductListItemModel>(),
                NewProducts = new List<ProductListItemModel>(),
                FeatureProducts = new List<ProductListItemModel>(),
            };

            var featureProducts = await _unitOfWork.Products.GetProductsAsync(p => p.IsFeatured == true, 1, 20);

            if (featureProducts == null || featureProducts.Count == 0)
            {
                return serviceResult;
            }

            foreach (var product in featureProducts)
            {
                homeData.Banner.Add(product.MainImageUrl);
            }

            homeData.FeatureProducts = featureProducts.ToListProductListItem();
            var hotProducts = await _productService.GetHotProducts();
            homeData.HotProducts = hotProducts.Data ?? new List<ProductListItemModel>();

            var newProducts = await _unitOfWork.Products.GetTopNewestProductsAsync(10);

            if (newProducts == null || newProducts.Count == 0)
            {
                return serviceResult;
            }

            homeData.NewProducts = newProducts.ToListProductListItem() ?? new List<ProductListItemModel>();

            serviceResult.Data = homeData;
            return serviceResult;
        }

        public async Task<ServiceResult<List<string>>> GetHotProdctsImage()
        {
            var serviceResult = new ServiceResult<List<string>>
            {
                IsSuccess = true,
                Data = new List<string>(),
                Message = Messenger.GetDataSuccessful
            };

            var featureProducts = await _unitOfWork.Products.GetProductsAsync(p => p.IsFeatured == true, 1, 20);

            if (featureProducts == null || featureProducts.Count == 0)
            {
                return serviceResult;
            }

            foreach (var product in featureProducts)
            {
                serviceResult.Data.Add(product.MainImageUrl);
            }

            return serviceResult;
        }
    }
}
