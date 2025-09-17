using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.Cart;

namespace TechStore.Service.Mappers
{
    public static class CartItemMappers
    {
        public static CartItemResponseModel ToCartItemResponseModel(this CartItem cartItem, ProductVariantOption productVariantOption)
        {
            return new CartItemResponseModel
            {
                CartItemId = cartItem.PublicId,
                CartId = cartItem.PublicId,
                ProductId = productVariantOption.ProductVariant.Product.PublicId,
                ProductName = productVariantOption.ProductVariant.Product.Name,
                MainImageUrl = productVariantOption.ProductVariant.Product.MainImageUrl,
                Quantity = cartItem.Quantity,
                PriceAtOrderTime = productVariantOption.ProductVariant.Price,
                Discount = cartItem.Discount,
                TotalPrice = cartItem.TotalPrice
            };
        }
    }
}
