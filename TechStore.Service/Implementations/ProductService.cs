using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
using TechStore.Model.DTOs.Order;
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

        public async Task<ServiceResult<PagedResult<ListItemProductModel>>> GetProductsAsync(ProductSearchQuery query)
        {
            var serviceResult = new ServiceResult<PagedResult<ListItemProductModel>>
            {
                IsSuccess = true,
                Data = new PagedResult<ListItemProductModel>
                {
                    CurrentPage = query.Page,
                    PageSize = query.PageSize,
                    TotalItems = 0,
                },
                Message = Messenger.GetDataSuccessful,
            };

            var pagedResult = await _uow.Products.SearchAsync(query);

            foreach (var item in pagedResult.Items)
            {
                serviceResult.Data.Items.Add(item.ToProductListItem());
            }

            serviceResult.Data.TotalItems = pagedResult.TotalItems;
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

        public async Task<ServiceResult<bool>> UpdateProduct(string id, ProductUpdateModel model)
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

            product.Name = model.Name;

            product.ShortDescription = model.ShortDescription ?? product.ShortDescription;
            product.Description = model.Description ?? product.Description;
            product.CategoryId = category.Id;
            product.BrandId = brand.Id;
            product.Tags = model.Tags ?? product.Tags;
            product.Warranty = model.Warranty;
            product.SaleStart = model.SaleStart;
            product.SalePrice = model.SalePrice ?? product.SalePrice;
            product.SaleEnd = model.SaleEnd;
            product.StartSellingDate = model.StartSellingDate ?? DateTime.UtcNow;
            product.EndSellingDate = model.EndSellingDate;
            product.IsFeatured = model.IsFeatured ?? product.IsFeatured;
            product.PublishDate = model.PublishDate;
            product.MainImageUrl = model.MainImageUrl;
            product.GalleryImageUrls = model.GalleryImageUrls;

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
                    Id = Guid.NewGuid(),
                    PublicId = await _sequenceService.GetNextProductIdAsync(),
                    BrandId = brand.Id,
                    BrandPublicId = brand.PublicId,
                    Brand = brand,
                    CategoryId = category.Id,
                    CategoryPublicId = category.PublicId,
                    Category = category,
                    Name = model.Name,
                    Slug = model.Slug ?? CommonFuntion.GenerateSlug(model.Name),
                    Tags = model.Tags,
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
                    MinPrice = 0,
                    MaxPrice = 0,
                    CreatedAt = TimeZoneHelper.GetUtcNow(),
                    EntityStatus = EEntityStatus.Active
                };

                foreach (var variantModel in model.Variants)
                {
                    var variant = new ProductVariant
                    {
                        Id = Guid.NewGuid(),
                        PublicId = $"{DateTime.Today:yyyyMMdd}{(Random.Shared.Next(100000, 1000000).ToString() + 1):D6}",
                        ProductId = product.Id,
                        Product = product,
                        Name = variantModel.Name,
                        Description = variantModel.Description,
                        Price = variantModel.Price,
                        ImportPrice = variantModel.ImportPrice,
                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                        EntityStatus = EEntityStatus.Active
                    };

                    if (variantModel.Options != null)
                    {
                        foreach (var optionModel in variantModel.Options)
                        {
                            var option = new ProductVariantOption
                            {
                                Id = Guid.NewGuid(),
                                PublicId = $"{DateTime.Today:yyyyMMdd}{(Random.Shared.Next(100000, 1000000).ToString() + 1):D6}",
                                ProductVariantId = variant.Id,
                                Name = optionModel.Name,
                                Price = optionModel.Price ?? variant.Price,
                                ImportPrice = optionModel.ImportPrice ?? variant.ImportPrice,
                                Stock = optionModel.Stock,
                                ImageUrl = optionModel.ImageUrl ?? CloudinaryFolders.DefaultImage,
                                CreatedAt = TimeZoneHelper.GetUtcNow(),
                                EntityStatus = EEntityStatus.Active
                            };

                            variant.Options.Add(option);
                        }
                    }

                    product.Variants.Add(variant);
                }

                product.MinPrice = product.Variants .SelectMany(v => v.Options).Min(o => o.Price);
                product.MaxPrice = product.Variants.SelectMany(v => v.Options).Max(o => o.Price);

                await _uow.Products.AddAsync(product);

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

            var product = await _uow.Products.Table.Where(p => p.PublicId == id)
                                                    .Include(p => p.Variants)
                                                        .ThenInclude(v => v.Options)
                                                    .FirstOrDefaultAsync();

            if (product == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            foreach (var variant in product.Variants)
            {
                foreach (var option in variant.Options)
                {
                    _uow.ProductVariantOptions.Remove(option);
                }
                _uow.ProductVariants.Remove(variant);
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

        
        public async Task<ServiceResult<AdminProductDetailModel>> GetAdminProductByIdAsync(string id)
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


        public async Task<ServiceResult<PagedResult<AdminListItemProduct>>> GetAdminProductsAsync(ProductSearchQuery query)
        {
            var serviceResult = new ServiceResult<PagedResult<AdminListItemProduct>>
            {
                IsSuccess = true,
                Data = new PagedResult<AdminListItemProduct>
                {
                    CurrentPage = query.Page,
                    PageSize = query.PageSize,
                    TotalItems = 0,
                },
                Message = Messenger.GetDataSuccessful,
            };

            var pagedResult = await _uow.Products.SearchAsync(query);

            foreach (var item in pagedResult.Items)
            {
                serviceResult.Data.Items.Add(item.ToAdminProductListItem());
            }

            serviceResult.Data.TotalItems = pagedResult.TotalItems;
            return serviceResult;
        }


        public async Task<ServiceResult<List<ListItemProductModel>>> GetUserRecommendedProducts(string userId)
        {
            var serviceResult = new ServiceResult<List<ListItemProductModel>>()
            {
                IsSuccess = true,
                Data = new List<ListItemProductModel>(),
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

        public async Task<ServiceResult<List<ListItemProductModel>>> GetHotProducts()
        {
            var serviceResult = new ServiceResult<List<ListItemProductModel>>
            {
                IsSuccess = true,
                Data = new List<ListItemProductModel>(),
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

        public async Task<ServiceResult<List<ListItemProductModel>>> GetFeaturedProducts()
        {
            var serviceResult = new ServiceResult<List<ListItemProductModel>>
            {
                IsSuccess = true,
                Data = new List<ListItemProductModel>(),
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

        public async Task<ServiceResult<string>> AddProductVariantAsync(string productId, ProductVariantCreateModel pvModel)
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
                Id = Guid.NewGuid(),
                PublicId = await _sequenceService.GetNextProductVariantIdAsync(),
                ProductId = product.Id,
                Name = pvModel.Name,
                Description = pvModel.Description,
                Price = pvModel.Price,
                ImportPrice = pvModel.ImportPrice,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
                EntityStatus = EEntityStatus.Active
            };

            foreach (var pvoModel in pvModel.Options)
            {
                variant.Options.Add(new ProductVariantOption
                {
                    PublicId = await _sequenceService.GetNextProductVariantOptionIdAsync(),
                    ProductVariantId = variant.Id,
                    Name = pvoModel.Name,
                    Price = pvoModel.Price ?? pvModel.Price,
                    ImportPrice = pvoModel.ImportPrice ?? pvModel.ImportPrice,
                    Stock = pvoModel.Stock,
                    ImageUrl = pvoModel.ImageUrl ?? CloudinaryFolders.DefaultImage,
                    CreatedAt = TimeZoneHelper.GetUtcNow(),
                    EntityStatus = EEntityStatus.Active
                });
            }

            product.Variants.Add(variant);
            product.MinPrice = product.Variants.SelectMany(v => v.Options).Min(o => o.Price);
            product.MaxPrice = product.Variants.SelectMany(v => v.Options).Max(o => o.Price);

            await _uow.ProductVariants.AddAsync(variant);
            _uow.Products.Update(product);
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
            productVariant.Description = string.IsNullOrEmpty(model.Description) ? productVariant.Description : model.Description;
            productVariant.Price = model.Price ?? productVariant.Price;
            productVariant.ImportPrice = model.ImportPrice ?? productVariant.ImportPrice;

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

            var product = await _uow.Products.GetByIdAsync(productId);
            if (product == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            var productVariant = await _uow.ProductVariants.GetByIdAsync(variantId);
            if (productVariant == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            var option = new ProductVariantOption
            {
                PublicId = await _sequenceService.GetNextProductVariantOptionIdAsync(),
                ProductVariantId = productVariant.Id,
                Name = model.Name,
                Price = model.Price ?? productVariant.Price,
                ImportPrice = model.ImportPrice ?? productVariant.ImportPrice,
                Stock = model.Stock,
                ImageUrl = model.ImageUrl ?? CloudinaryFolders.DefaultImage,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
                EntityStatus = EEntityStatus.Active
            };


            await _uow.ProductVariantOptions.AddAsync(option);
            product.MinPrice = product.Variants.SelectMany(v => v.Options).Min(o => o.Price);
            product.MaxPrice = product.Variants.SelectMany(v => v.Options).Max(o => o.Price);
            _uow.Products.Update(product);


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

            var product = await _uow.Products.Table.Where(p => p.PublicId == productId)
                                                    .Include(p => p.Variants)
                                                        .ThenInclude(v => v.Options)
                                                    .FirstOrDefaultAsync();
            if (product == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            var productVariantOption = await _uow.ProductVariantOptions.GetByIdAsync(optionId);

            if (productVariantOption == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            productVariantOption.UpdatedAt = TimeZoneHelper.GetUtcNow();
            productVariantOption.Name = string.IsNullOrEmpty(model.Name) ? productVariantOption.Name : model.Name;
            productVariantOption.ImageUrl = string.IsNullOrEmpty(model.ImageUrl) ? productVariantOption.ImageUrl : model.ImageUrl;
            productVariantOption.Price = model.Price;
            productVariantOption.ImportPrice = model.ImportPrice;
            productVariantOption.Stock = model.Stock;

            _uow.ProductVariantOptions.Update(productVariantOption);
            product.MinPrice = product.Variants.SelectMany(v => v.Options).Min(o => o.Price);
            product.MaxPrice = product.Variants.SelectMany(v => v.Options).Max(o => o.Price);
            _uow.Products.Update(product);

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
