using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Helpers;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Cart;
using TechStore.Service.Interfaces;
using TechStore.Service.Mappers;

namespace TechStore.Service.Implementations
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _uow;
        private readonly SequenceGeneratorService _sequenceService;

        public CartService(IUnitOfWork uow,
            SequenceGeneratorService sequenceService
            )
        {
            _uow = uow;
            _sequenceService = sequenceService;
        }

        public async Task<ServiceResult<CartItemResponseModel>> AddToCart(string userId, CartItemUpdateModel model)
        {
            var serviceResult = new ServiceResult<CartItemResponseModel>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.SystemError,
            };

            if (model.Quantity < 1)
            {
                serviceResult.Message = Messenger.BadRequest;
                return serviceResult;
            }

            var productVariantOption = await _uow.ProductVariantOptions.GetProductVariantOptionDetailByPublicIdAsync(model.ProductVariantOptionId);
            var user = await _uow.Users.GetByIdAsync(userId);

            if (productVariantOption == null || user == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            //var existItem = await _uow.CartItems.ExistsByGuidAsync(nameof(CartItem.ProductId), productVariantOption.Id);
            var existItem = await _uow.CartItems.FindOneAsync(i => i.ProductVariantOptionId == productVariantOption.Id);

            if (existItem == null)
            {
                CartItem cartItem = new CartItem
                {
                    UserId = user.Id,
                    PublicId = userId + "-" + productVariantOption.PublicId,
                    ProductVariantOptionId = productVariantOption.Id,
                    Quantity = model.Quantity,
                    TotalPrice = model.Quantity * productVariantOption.Price,
                    Discount = 0,
                    CreatedAt = TimeZoneHelper.GetUtcNow(),
                    EntityStatus = Common.Enums.EEntityStatus.Active,
                };

                await _uow.CartItems.AddAsync(cartItem);

                var result = await _uow.CommitAsync();

                if (result < 1)
                {
                    return serviceResult;
                }

                serviceResult.Data = cartItem.ToCartItemResponseModel(productVariantOption);
            }
            else
            {
                serviceResult.Data = existItem.ToCartItemResponseModel(productVariantOption);
            }

            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.SuccessFull;

            return serviceResult;
        }

        public async Task<ServiceResult<List<CartItemResponseModel>>> RemoveCartItems(string userId, List<string> listCartItemId)
        {
            var serviceResult = new ServiceResult<List<CartItemResponseModel>>
            {
                IsSuccess = false,
                Data = new List<CartItemResponseModel>(),
                Message = Messenger.SystemError,
            };

            var user = await _uow.Users.GetByIdAsync(userId);

            if (user == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            foreach (var cartItemId in listCartItemId)
            {
                var cartItem = await _uow.CartItems.GetByIdAsync(cartItemId);
                if (cartItem != null)
                {
                    var productVariantOption = await _uow.ProductVariantOptions.GetProductVariantOptionDetailByInternalIdAsync(cartItem.ProductVariantOptionId);
                    if (productVariantOption != null)
                    {
                        serviceResult.Data.Add(cartItem.ToCartItemResponseModel(productVariantOption));
                    }
                    _uow.CartItems.Remove(cartItem);
                }
            }

            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.SuccessFull;

            return serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdateCart(string userId, CartItemUpdateModel model)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.SystemError,
            };

            var product = await _uow.ProductVariantOptions.GetByIdAsync(model.ProductVariantOptionId);
            var user = await _uow.Users.GetByIdAsync(userId);

            if (product == null || user == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            var cartItem = await _uow.CartItems.GetByIdAsync(product.PublicId);

            if (cartItem == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            cartItem.Quantity = model.Quantity;
            cartItem.TotalPrice = model.Quantity * product.Price;
            cartItem.UpdatedAt = TimeZoneHelper.GetUtcNow();

            _uow.CartItems.Update(cartItem);

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

        public async Task<ServiceResult<bool>> ClearCart(string userId)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.SystemError
            };

            var user = await _uow.Users.GetByIdAsync(userId);

            if (user == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            var cartItems = await _uow.CartItems.FindManyAsync(c => c.UserId == user.Id);

            if (cartItems == null)
            {
                return serviceResult;
            }

            _uow.CartItems.RemoveRange(cartItems);

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

        public async Task<ServiceResult<List<CartItemResponseModel>>> GetCartItems(string userId, int pageNumber, int pageSize)
        {
            var serviceResult = new ServiceResult<List<CartItemResponseModel>>
            {
                IsSuccess = true,
                Data = new List<CartItemResponseModel>(),
                Message = Messenger.GetDataSuccessful
            };

            var user = await _uow.Users.GetByIdAsync(userId);

            if (user == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            var cartItems = await _uow.CartItems.FindManyWithNumberAsync(c => c.UserId == user.Id, pageNumber, pageSize);

            foreach ( var cartItem in cartItems)
            {
                var productVariantOption = await _uow.ProductVariantOptions.GetProductVariantOptionDetailByInternalIdAsync(cartItem.ProductVariantOptionId);
                if (productVariantOption != null)
                {
                    serviceResult.Data.Add(cartItem.ToCartItemResponseModel(productVariantOption));
                }
            }

            return serviceResult;
        }
    }
}

