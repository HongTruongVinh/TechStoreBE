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
        private readonly IUnitOfWork _uow;
        //private readonly IProductService _productService;
        public HomeService(
            IUnitOfWork unitOfWork
            //IOrderService orderService,
            //IProductService productService,
            //IProductRepository productRepository
            )
        {
            //_productService = productService;
            _uow = unitOfWork;
        }

        public async Task<ServiceResult<HomeResponseModel>> GetHomeProduct()
        {
            var serviceResult = new ServiceResult<HomeResponseModel>
            {
                IsSuccess = true,
                Data = null,
                Message = Messenger.GetDataSuccessful
            };

            //var homeData = new HomeResponseModel
            //{
            //    Banner = new List<string>(),
            //    HotProducts = new List<ProductListItemModel>(),
            //    NewProducts = new List<ProductListItemModel>(),
            //    FeatureProducts = new List<ProductListItemModel>(),
            //};

            //var featureProducts = await _uow.Products.GetProductsAsync(p => p.IsFeatured == true, 1, 20);

            //if (featureProducts == null || featureProducts.Count == 0)
            //{
            //    return serviceResult;
            //}

            //foreach (var product in featureProducts)
            //{
            //    homeData.Banner.Add(product.MainImageUrl);
            //}

            //foreach (var product in featureProducts)
            //{
            //    //foreach(var variant in product.Variants)
            //    //{
            //    //    homeData.FeatureProducts.Add(variant.ToProductListItem());
            //    //}
            //    homeData.FeatureProducts.Add(product.ToProductListItem());
            //}
            //var hotProducts = await _productService.GetHotProducts();
            //homeData.HotProducts = hotProducts.Data ?? new List<ProductListItemModel>();

            //var newProducts = await _uow.Products.GetTopNewestProductsAsync(10);

            //if (newProducts == null || newProducts.Count == 0)
            //{
            //    return serviceResult;
            //}

            //foreach (var product in newProducts)
            //{
            //    //foreach (var variant in product.Variants)
            //    //{
            //    //    homeData.NewProducts.Add(variant.ToProductListItem());
            //    //}
            //    homeData.NewProducts.Add(product.ToProductListItem());
            //}

            //serviceResult.Data = homeData;
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

            var featureProducts = await _uow.Products.GetProductsAsync(p => p.IsFeatured == true, 1, 20);

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

        public async Task<ServiceResult<List<ListItemProductModel>>> GetFeaturedProducts()
        {
            var serviceResult = new ServiceResult<List<ListItemProductModel>>
            {
                IsSuccess = true,
                Data = new List<ListItemProductModel>(),
                Message = Messenger.GetDataSuccessful
            };

            var products = await _uow.Products.GetProductsAsync(p => p.IsFeatured, 1, 16);
            if (products != null)
            {
                foreach (var product in products)
                {
                    serviceResult.Data.Add(product.ToProductListItem());
                }
            }

            return serviceResult;
        }

        public async Task<ServiceResult<List<ListItemProductModel>>> GetProductsByBrandName(string brandName, int page, int pageSize)
        {
            var serviceResult = new ServiceResult<List<ListItemProductModel>>
            {
                IsSuccess = true,
                Data = new List<ListItemProductModel>(),
                Message = Messenger.NoExitData
            };

            var brand = await _uow.Brands.FindOneAsync(b => b.Name.ToLower().Contains(brandName.ToLower()));

            if (brand == null)
            {
                return serviceResult;
            }

            var products = await _uow.Products.GetProductsAsync(p => p.BrandId == brand.Id, page, pageSize);
            if (products != null)
            {
                foreach (var product in products)
                {
                    serviceResult.Data.Add(product.ToProductListItem());
                }
            }

            serviceResult.Message = Messenger.GetDataSuccessful;
            return serviceResult;
        }

        public async Task<ServiceResult<List<ListItemProductModel>>> GetSamsungProducts()
        {
            var serviceResult = new ServiceResult<List<ListItemProductModel>>
            {
                IsSuccess = true,
                Data = new List<ListItemProductModel>(),
                Message = Messenger.NoExitData
            };

            var brand = await _uow.Brands.FindOneAsync(b => b.Name.ToLower() == "samsung");

            if (brand == null)
            {
                return serviceResult;
            }

            var products = await _uow.Products.GetProductsAsync(p => p.BrandId == brand.Id, 1, 16);
            if (products != null)
            {
                foreach (var product in products)
                {
                    serviceResult.Data.Add(product.ToProductListItem());
                }
            }

            serviceResult.Message = Messenger.GetDataSuccessful;
            return serviceResult;
        }
    }
}
