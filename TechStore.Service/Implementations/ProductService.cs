using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.CommonFunction;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Helpers;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Product;
using TechStore.Model.DTOs.ProductVariant;
using TechStore.Model.DTOs.ProductVariantOption;
using TechStore.Service.Interfaces;
using TechStore.Service.Mappers;

namespace TechStore.Service.Implementations
{
    public class ProductService : IProductService
    {

        private readonly IUnitOfWork _uow;
        private readonly SequenceGeneratorService _sequenceService;

        public ProductService(IUnitOfWork uow, SequenceGeneratorService sequenceService)
        {
            _uow = uow;
            _sequenceService = sequenceService;
        }

        public async Task<ServiceResult<List<ProductListItemModel>>> GetProductsAsync(int page, int pageSize)
        {
            var serviceResult = new ServiceResult<List<ProductListItemModel>>
            {
                IsSuccess = true,
                Data = null,
                Message = Messenger.NoExitData
            };

            List<ProductListItemModel> productModels = new List<ProductListItemModel>();

            var products = await _uow.Products.GetProductsAsync(p => true, page, pageSize);

            if (products == null)
            {
                return serviceResult;
            }

            foreach (var product in products)
            {
                //foreach (var variant in product.Variants)
                //{
                //    productModels.Add(variant.ToProductListItem());
                //}
                productModels.Add(product.ToProductListItem());
            }

            serviceResult.Data = productModels;
            serviceResult.Message = Messenger.GetDataSuccessful;

            return serviceResult;
        }

        public async Task<ServiceResult<List<ProductListItemModel>>> GetProductsFilteredAsync(
            int pageNumber,
            int pageSize,
            string? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? brandId = null)
        {
            var serviceResult = new ServiceResult<List<ProductListItemModel>>
            {
                IsSuccess = true,
                Data = null,
                Message = Messenger.NoExitData
            };

            List<ProductListItemModel> productModels = new List<ProductListItemModel>();

            var products = await _uow.Products.GetProductsFilteredAsync(pageNumber, pageSize, categoryId, minPrice, maxPrice, brandId);

            if (products == null)
            {
                return serviceResult;
            }

            foreach (var product in products)
            {
                //foreach (var variant in product.Variants)
                //{
                //    productModels.Add(variant.ToProductListItem());
                //}

                productModels.Add(product.ToProductListItem());
            }


            serviceResult.Data = productModels;
            serviceResult.Message = Messenger.GetDataSuccessful;

            return serviceResult;
        }

        public async Task<ServiceResult<ProductDetailModel>> GetProductById(string id)
        {
            var serviceResult = new ServiceResult<ProductDetailModel>
            {
                IsSuccess = true,
                Data = null,
                Message = Messenger.NoExitData
            };

            var product = await _uow.Products.GetProductWithDetailsByIdAsync(id);
            if (product == null)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = product.ToProductDetail();
            serviceResult.Message = Messenger.GetDataSuccessful;

            return serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdateProductInformation(string id, ProductUpdateModel productUpdateModel)
        {
            var serviceResult = new ServiceResult<bool>()
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.UpdateDataError
            };

            var product = await _uow.Products.GetByIdAsync(id);

            if (product == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            var category = await _uow.Categories.GetByIdAsync(productUpdateModel.CategoryId);
            if (category == null)
            {
                serviceResult.Message = Messenger.UpdateDataError;
                return serviceResult;
            }

            var brand = await _uow.Brands.GetByIdAsync(productUpdateModel.BrandId);
            if (brand == null)
            {
                serviceResult.Message = Messenger.UpdateDataError;
                return serviceResult;
            }

            product.Name = productUpdateModel.Name;
            product.ShortDescription = productUpdateModel.ShortDescription;
            product.Description = productUpdateModel.Description;
            product.CategoryId = category.Id;
            product.BrandId = brand.Id;
            product.Tags = productUpdateModel.Tag;
            product.SaleStart = productUpdateModel.SaleStart;
            product.SalePrice = productUpdateModel.SalePrice;
            product.SaleEnd = productUpdateModel.SaleEnd;
            product.StartSellingDate = productUpdateModel.StartSellingDate ?? DateTime.UtcNow;
            product.EndSellingDate = productUpdateModel.EndSellingDate;
            product.IsFeatured = productUpdateModel.IsFeatured;
            product.PublishDate = productUpdateModel.PublishDate;

            if (productUpdateModel.MainImageUrl != null)
            {
                product.MainImageUrl = productUpdateModel.MainImageUrl;
            }

            if (productUpdateModel.GalleryImageUrls != null)
            {
                product.GalleryImageUrls = productUpdateModel.GalleryImageUrls;
            }

            _uow.Products.Update(product);

            var result = await _uow.CommitAsync();
            if (result < 1)
            {
                serviceResult.Message = Messenger.SystemError;
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.SuccessFull;

            return serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdateProductCount(string id, ProductCountUpdateModel model)
        {
            var serviceResult = new ServiceResult<bool>()
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.UpdateDataError
            };

            var option = await _uow.ProductVariantOptions.GetByIdAsync(id);

            if (option == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            option.Stock += model.Quantity;

            _uow.ProductVariantOptions.Update(option);

            var result = await _uow.CommitAsync();
            if (result < 1)
            {
                serviceResult.Message = Messenger.SystemError;
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.SuccessFull;

            return serviceResult;
        }

        public async Task<ServiceResult<string>> AddProduct(ProductCreateModel model)
        {
            var serviceResult = new ServiceResult<string>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.CreateDataError
            };

            try
            {
                if (model.Variants.Count < 1)
                {
                    serviceResult.Message = Messenger.IncorrectDataFormat;
                    return serviceResult;
                }

                var category = await _uow.Categories.GetByIdAsync(model.CategoryId);
                if (category == null)
                {
                    serviceResult.Message = Messenger.UpdateDataError;
                    return serviceResult;
                }

                var brand = await _uow.Brands.GetByIdAsync(model.BrandId);
                if (brand == null)
                {
                    serviceResult.Message = Messenger.UpdateDataError;
                    return serviceResult;
                }

                var product = new Product
                {
                    PublicId = await _sequenceService.GetNextProductIdAsync(),
                    BrandId = brand.Id,
                    Brand = brand,
                    CategoryId = category.Id,
                    Category = category,
                    Name = model.Name,
                    Slug = model.Slug ?? CommonFuntion.GenerateSlug(model.Name),
                    Tags = model.Tag,
                    IsFeatured = model.IsFeatured,
                    ShortDescription = model.ShortDescription,
                    Description = model.Description,
                    Warranty = model.Warranty,
                    PublishDate = model.PublishDate,
                    SalePrice = 0,
                    SaleStart = model.SaleStart,
                    SaleEnd = model.SaleEnd,
                    MainImageUrl = model.MainImageUrl ?? CloudinaryFolders.DefaultImage,
                    GalleryImageUrls = model.GalleryImageUrls,
                    StartSellingDate = model.StartSellingDate ?? DateTime.UtcNow,
                    EndSellingDate = model.EndSellingDate,
                    CreatedAt = TimeZoneHelper.GetUtcNow(),
                    EntityStatus = EEntityStatus.Active
                };

                await _uow.Products.AddAsync(product);
                //await _uow.CommitAsync();

                foreach (var variantModel in model.Variants)
                {
                    var variant = new ProductVariant
                    {
                        //PublicId = await _sequenceService.GetNextProductVariantIdAsync(),
                        PublicId = Random.Shared.Next(100000, 999999).ToString(),
                        ProductId = product.Id,
                        Product = product,
                        Name = variantModel.Name,
                        Description = variantModel.Description,
                        Price = variantModel.Price,
                        ImportPrice = variantModel.ImportPrice,
                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                        EntityStatus = EEntityStatus.Active
                    };
                    await _uow.ProductVariants.AddAsync(variant);
                    //await _uow.CommitAsync();

                    if (variantModel.Options != null)
                    {
                        foreach (var optionModel in variantModel.Options)
                        {
                            var option = new ProductVariantOption
                            {
                                //PublicId = await _sequenceService.GetNextProductVariantOptionIdAsync(),
                                PublicId = Random.Shared.Next(100000, 999999).ToString(),
                                ProductVariantId = variant.Id,
                                Name = optionModel.Name,
                                Price = optionModel.Price ?? 0,
                                Stock = optionModel.Stock,
                                ImageUrl = optionModel.ImageUrl ?? CloudinaryFolders.DefaultImage,
                                CreatedAt = TimeZoneHelper.GetUtcNow(),
                                EntityStatus = EEntityStatus.Active
                            };
                            await _uow.ProductVariantOptions.AddAsync(option);
                            //await _uow.CommitAsync();
                        }
                    }
                }

                var result = await _uow.CommitAsync();
                if (result < 1)
                {
                    return serviceResult;
                }

                serviceResult.IsSuccess = true;
                serviceResult.Data = product.PublicId;
                serviceResult.Message = Messenger.SuccessFull;
                return serviceResult;
            }
            catch
            {
                return serviceResult;
            }

            //return serviceResult;
        }

        public async Task<ServiceResult<bool>> DeleteProduct(string id)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.SystemError
            };

            var product = await _uow.Products.GetByIdAsync(id);

            if (product == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            _uow.Products.Remove(product);

            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.Data = true;
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.SuccessFull;

            return serviceResult;
        }

        public async Task<ServiceResult<bool>> InActiveProduct(string id)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.SystemError
            };

            var product = await _uow.Products.GetByIdAsync(id);

            if (product == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            product.EntityStatus = EEntityStatus.Deleted;

            _uow.Products.Update(product);

            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.Data = true;
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.SuccessFull;

            return serviceResult;
        }

        //public async Task<List<ProductResponseModel>> GetProductByCategoryId(string categoryId)
        //{
        //    List<ProductResponseModel> productModels = new List<ProductResponseModel>();

        //    var products = await _productRepository.GetProductsByCategoryIdAsync(categoryId);

        //    foreach (var product in products)
        //    {
        //        var category = await _categoryRepository.GetByIdAsync(product.CategoryId);
        //        var brand = await _brandRepository.GetByIdAsync(product.BrandId);
        //        var ordersCount = await _orderRepository.GetOrdersCountOfProductAsync(product.ProductId);

        //        ProductResponseModel productResponseModel = new ProductResponseModel
        //        {
        //            ProductId = product.ProductId,
        //            Category = new CategoryResponseModel
        //            {
        //                CategoryId = category.CategoryId,
        //                Name = category.Name,
        //                Description = category.Description,
        //                Slug = category.Slug,
        //                IconImageUrl = category.IconImageUrl
        //            },
        //            Brand = new BrandResponseModel
        //            {
        //                BrandId = brand.BrandId,
        //                Name = brand.Name,
        //                Description = brand.Description,
        //                Slug = brand.Slug,
        //                IconImageUrl = brand.IconImageUrl
        //            },
        //            Name = product.Name,
        //            ShortDescription = product.ShortDescription,
        //            Description = product.Description,
        //            Stock = product.Stock,
        //            Price = product.Price,
        //            SaleStart = product.SaleStart,
        //            SaleEnd = product.SaleEnd,
        //            MainImageUrl = product.MainImageUrl,
        //            GalleryImageUrls = product.GalleryImageUrls,
        //            StartSellingDate = product.CreatedAt ?? TimeZoneHelper.GetUtcNow(),
        //            SoldCount = product.SoldCount,
        //            AverageRating = product.AverageRating,
        //            SalePrice = product.SalePrice,
        //            IsOnSale = product.IsOnSale,
        //            IsFeatured = product.IsFeatured,
        //        };

        //        productModels.Add(productResponseModel);
        //    }

        //    return productModels;
        //}

        public async Task<ServiceResult<AdminProductDetailModel>> GetAdminProductById(string id)
        {
            var serviceResult = new ServiceResult<AdminProductDetailModel>
            {
                IsSuccess = true,
                Data = null,
                Message = Messenger.NoExitData
            };

            var product = await _uow.Products.GetProductWithDetailsByIdAsync(id);

            if (product == null)
            {
                return serviceResult;
            }

            var category = product.Category;
            if (category == null)
            {
                return serviceResult;
            }

            var brand = product.Brand;
            if (brand == null)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = product.ToAdminProductDetail(category, brand);
            serviceResult.Message = Messenger.GetDataSuccessful;

            return serviceResult;
        }


        public async Task<ServiceResult<List<AdminProductDetailModel>>> GetAdminProducts(int pageNumber, int pageSize)
        {
            var serviceResult = new ServiceResult<List<AdminProductDetailModel>>
            {
                IsSuccess = true,
                Data = new List<AdminProductDetailModel>(),
                Message = Messenger.NoExitData
            };

            var products = await _uow.Products.GetProductsAsync(p => true, pageNumber, pageSize);

            if (products == null || products.Count == 0)
            {
                return serviceResult;
            }

            foreach (var product in products)
            {
                var category = product.Category;
                if (category == null)
                {
                    category = await _uow.Categories.GetByIdAsync(product.Category.PublicId);
                    if (category == null)
                    {
                        continue;
                    }
                }

                var brand = product.Brand;
                if (brand == null)
                {
                    brand = await _uow.Brands.GetByIdAsync(product.Brand.PublicId);
                    if (brand == null)
                    {
                        continue;
                    }
                }

                serviceResult.Data.Add(product.ToAdminProductDetail(category, brand));
            }

            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.GetDataSuccessful;

            return serviceResult;
        }


        public async Task<ServiceResult<List<ProductListItemModel>>> GetUserRecommendedProducts(string userId)
        {
            var serviceResult = new ServiceResult<List<ProductListItemModel>>()
            {
                IsSuccess = true,
                Data = new List<ProductListItemModel>(),
                Message = Messenger.GetDataSuccessful
            };

            //var customer = await _uow.Users.GetByIdAsync(userId);

            //if (customer == null)
            //{
            //    var hotProducts = await GetHotProducts();
            //    serviceResult.Data = hotProducts.Data;
            //    return serviceResult;
            //}

            //var orders = await _uow.Orders.FindManyAsync(p => p.CustomerId == customer.Id);

            //if (orders == null)
            //{
            //    var hotProducts = await GetHotProducts();
            //    serviceResult.Data = hotProducts.Data;
            //    return serviceResult;
            //}

            //foreach (var order in orders)
            //{
            //    var orderItems = await _uow.OrderItems.FindManyAsync(oi => oi.OrderId == order.Id);

            //    var categoryCounts = orderItems
            //        .GroupBy(oi => oi.Product.Category.Name)
            //        .Select(g => new { CategoryName = g.Key, Count = g.Count() })
            //        .OrderByDescending(g => g.Count)
            //        .FirstOrDefault();

            //    if (categoryCounts == null)
            //    {
            //        var hotProducts = await GetHotProducts();
            //        serviceResult.Data = hotProducts.Data;
            //        return serviceResult;
            //    }

            //    var categories = new List<Category>();

            //    for (int i = 0; i < categories.Count; i++)
            //    {
            //        var categoryCount = categories[i];

            //        var category = await _uow.Categories.FindOneAsync(c => c.Name == categoryCount.Name);

            //        if (category != null)
            //        {
            //            categories.Add(category);
            //        }
            //    }

            //    foreach (var category in categories)
            //    {
            //        var products = await _uow.Products.FindManyAsync(p => p.Category.Id == category.Id);

            //        if (serviceResult.Data.Count > 10)
            //        {
            //            break;
            //        }

            //        foreach (var product in products)
            //        {
            //            serviceResult.Data.Add(product.ToProductListItem());
            //        }
            //    }

            //}

            return serviceResult;
        }

        public async Task<ServiceResult<List<ProductListItemModel>>> GetHotProducts()
        {
            var serviceResult = new ServiceResult<List<ProductListItemModel>>
            {
                IsSuccess = true,
                Data = new List<ProductListItemModel>(),
                Message = Messenger.GetDataSuccessful
            };

            //var orders = await _orderService.GetAllOrdersAsync();
            //var products = await _productRepository.GetAllAsync();

            //if (orders.Data == null)
            //{
            //    return serviceResult;
            //}

            //var recentTop15Products = orders.Data
            //    .Where(o => o.CreatedAt >= DateTime.UtcNow.AddDays(-30))
            //    .SelectMany(o => o.Items)
            //    .GroupBy(item => new { item.ProductId, item.ProductName, item.CategoryName })
            //    .Select(g => new
            //    {
            //        ProductId = g.Key.ProductId,
            //        Name = g.Key.ProductName,
            //        CategoryName = g.Key.CategoryName,
            //        SoldCount = g.Sum(x => x.Quantity)
            //    })
            //    .OrderByDescending(x => x.SoldCount)
            //    .Take(6)
            //    .ToList();

            //if (recentTop15Products.Count > 0)
            //{
            //    foreach (var item in recentTop15Products)
            //    {
            //        var product = products.Where(x => x.ProductId == item.ProductId).FirstOrDefault();

            //        if (product != null)
            //        {
            //            var productModel = new ProductListItemModel
            //            {
            //                ProductId = product.ProductId,
            //                CategoryName = item.CategoryName,
            //                Name = product.Name,
            //                SoldCount = item.SoldCount,
            //                Price = product.Price,
            //                MainImageUrl = product.MainImageUrl,
            //                ShortDescription = product.ShortDescription,
            //                AverageRating = product.AverageRating,
            //                Stock = product.Stock,
            //                RatedCount = product.RatedCount,
            //            };
            //            serviceResult.Data.Add(productModel);
            //        }
            //    }
            //}

            return serviceResult;
        }

        public async Task<ServiceResult<List<ProductListItemModel>>> SearchByNameAsync(string keyword, int pageNumber, int pageSize)
        {
            var serviceResult = new ServiceResult<List<ProductListItemModel>>
            {
                IsSuccess = true,
                Data = new List<ProductListItemModel>(),
                Message = Messenger.GetDataSuccessful
            };

            var products = await _uow.Products.SearchByNameAsync(keyword, pageNumber, pageSize);
            if (products != null)
            {
                foreach (var product in products)
                {
                    //foreach (var variant in product.Variants)
                    //{
                    //    serviceResult.Data.Add(variant.ToProductListItem());
                    //}
                    serviceResult.Data.Add(product.ToProductListItem());
                }
            }

            return serviceResult;
        }

        public async Task<ServiceResult<List<ProductListItemModel>>> GetProductsByCategory(string categorySlug, int pageNumber, int pageSize)
        {
            var serviceResult = new ServiceResult<List<ProductListItemModel>>
            {
                IsSuccess = true,
                Data = new List<ProductListItemModel>(),
                Message = Messenger.NoExitData
            };

            var category = await _uow.Categories.GetBySlugAsync(categorySlug);
            if (category == null)
            {
                return serviceResult;
            }

            var products = await _uow.Products.GetProductsByCategoryAsync(category.Id, pageNumber, pageSize);
            if (products != null)
            {
                foreach (var product in products)
                {
                    //foreach (var variant in product.Variants)
                    //{
                    //    serviceResult.Data.Add(variant.ToProductListItem());
                    //}
                    serviceResult.Data.Add(product.ToProductListItem());
                }
            }

            return serviceResult;
        }

        public async Task<ServiceResult<List<ProductListItemModel>>> GetProductsByCategoryAndBrand(string categorySlug, string brandSlug, int pageNumber, int pageSize)
        {
            var serviceResult = new ServiceResult<List<ProductListItemModel>>
            {
                IsSuccess = true,
                Data = new List<ProductListItemModel>(),
                Message = Messenger.NoExitData
            };

            var category = await _uow.Categories.GetBySlugAsync(categorySlug);
            var brand = await _uow.Brands.GetBySlugAsync(brandSlug);
            if (category == null || brand == null)
            {
                return serviceResult;
            }

            var products = await _uow.Products.GetProductsByCategoryAndBrandAsync(category.Id, brand.Id, pageNumber, pageSize);
            if (products != null)
            {
                foreach (var product in products)
                {
                    //foreach (var variant in product.Variants)
                    //{
                    //    serviceResult.Data.Add(variant.ToProductListItem());
                    //}
                    serviceResult.Data.Add(product.ToProductListItem());
                }
            }

            return serviceResult;
        }

        public async Task<ServiceResult<List<ProductListItemModel>>> GetFeaturedProducts()
        {
            var serviceResult = new ServiceResult<List<ProductListItemModel>>
            {
                IsSuccess = true,
                Data = new List<ProductListItemModel>(),
                Message = Messenger.GetDataSuccessful
            };

            var products = await _uow.Products.GetFeatureProductsWithDetailsAsync(1, 16);
            if (products != null)
            {
                foreach (var product in products)
                {
                    serviceResult.Data.Add(product.ToProductListItem());
                }
            }

            return serviceResult;
        }

        public async Task<ServiceResult<string>> AddProductVariantAsync(string productId, ProductVariantCreateModel model)
        {
            var serviceResult = new ServiceResult<string>
            {
                IsSuccess = false,
                Data = Messenger.CreateDataError,
                Message = Messenger.SystemError
            };

            var product = await _uow.Products.GetByIdAsync(productId);

            if (product == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            var variant = new ProductVariant
            {
                PublicId = await _sequenceService.GetNextProductVariantIdAsync(),
                ProductId = product.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ImportPrice = model.ImportPrice,
                SalePrice = model.SalePrice,
                SaleStart = model.SaleStart,
                SaleEnd = model.SaleEnd,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
                EntityStatus = EEntityStatus.Active
            };
            await _uow.ProductVariants.AddAsync(variant);

            if (model.Options != null)
            {
                foreach (var optionModel in model.Options)
                {
                    var option = new ProductVariantOption
                    {
                        PublicId = await _sequenceService.GetNextProductVariantOptionIdAsync(),
                        ProductVariantId = variant.Id,
                        Name = optionModel.Name,
                        Price = optionModel.Price ?? 0,
                        Stock = optionModel.Stock,
                        ImageUrl = optionModel.ImageUrl ?? CloudinaryFolders.DefaultImage,
                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                        EntityStatus = EEntityStatus.Active
                    };
                    await _uow.ProductVariantOptions.AddAsync(option);
                }
            }

            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = Messenger.SuccessFull;
            serviceResult.Message = Messenger.SuccessFull;
            return serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdateProductVariantAsync(string productId, string variantId, ProductVariantUpdateModel model)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.SystemError
            };

            var productVariant = await _uow.ProductVariants.GetByIdAsync(variantId);

            if (productVariant == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            productVariant.UpdatedAt = TimeZoneHelper.GetUtcNow();
            productVariant.Name = string.IsNullOrEmpty(model.Name) ? productVariant.Name : model.Name;
            productVariant.Price = model.Price ?? productVariant.Price;
            productVariant.ImportPrice = model.ImportPrice ?? productVariant.ImportPrice;
            productVariant.SalePrice = model.SalePrice ?? productVariant.SalePrice;
            productVariant.SaleStart = model.SaleStart ?? productVariant.SaleStart;
            productVariant.SaleEnd = model.SaleEnd ?? productVariant.SaleEnd;

            _uow.ProductVariants.Update(productVariant);
            var result = await _uow.CommitAsync();

            if(result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;
            return serviceResult;
        }

        public async Task<ServiceResult<bool>> DeleteProductVariantAsync(string productId, string variantId)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.SystemError
            };

            var productVariant = await _uow.ProductVariants.GetByIdAsync(variantId);

            if (productVariant == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            //productVariant.UpdatedAt = TimeZoneHelper.GetUtcNow();
            //productVariant.EntityStatus = EEntityStatus.Deleted;

            _uow.ProductVariants.Remove(productVariant);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.SuccessFull;
            return serviceResult;
        }

        public async Task<ServiceResult<string>> AddProductVariantOptionAsync(string productId, string variantId, ProductVariantOptionCreateModel model)
        {
            var serviceResult = new ServiceResult<string>
            {
                IsSuccess = false,
                Data = Messenger.CreateDataError,
                Message = Messenger.SystemError
            };

            var productVariantOption = await _uow.ProductVariants.GetByIdAsync(variantId);

            if (productVariantOption == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            var option = new ProductVariantOption
            {
                PublicId = await _sequenceService.GetNextProductVariantOptionIdAsync(),
                ProductVariantId = productVariantOption.Id,
                Name = model.Name,
                Price = model.Price ?? 0,
                Stock = model.Stock,
                ImageUrl = model.ImageUrl ?? CloudinaryFolders.DefaultImage,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
                EntityStatus = EEntityStatus.Active
            };

            await _uow.ProductVariantOptions.AddAsync(option);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = Messenger.SuccessFull;
            serviceResult.Message = Messenger.SuccessFull;
            return serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdateProductVariantOptionAsync(string productId, string variantId, string optionId, ProductVariantOptionUpdateModel model)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.SystemError
            };

            var productVariantOption = await _uow.ProductVariantOptions.GetByIdAsync(optionId);

            if (productVariantOption == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            productVariantOption.UpdatedAt = TimeZoneHelper.GetUtcNow();
            productVariantOption.Name = string.IsNullOrEmpty(model.Name) ? productVariantOption.Name : model.Name;
            productVariantOption.ImageUrl = string.IsNullOrEmpty(model.ImageUrl) ? productVariantOption.ImageUrl : model.ImageUrl;
            productVariantOption.Price = model.Price ?? productVariantOption.Price;
            productVariantOption.Stock = model.Stock ?? productVariantOption.Stock;

            _uow.ProductVariantOptions.Update(productVariantOption);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;
            return serviceResult;
        }

        public async Task<ServiceResult<bool>> DeleteProductVariantOptionAsync(string productId, string variantId, string optionId)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.SystemError
            };

            var productVariantOption = await _uow.ProductVariantOptions.GetByIdAsync(optionId);

            if (productVariantOption == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            //productVariantOption.UpdatedAt = TimeZoneHelper.GetUtcNow();
            //productVariantOption.EntityStatus = EEntityStatus.Deleted;
            

            _uow.ProductVariantOptions.Remove(productVariantOption);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.SuccessFull;
            return serviceResult;
        }
    }
}
