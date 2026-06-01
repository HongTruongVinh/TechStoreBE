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
                Id = cartItem.PublicId,
                Slug = productVariantOption.ProductVariant.Product.Slug,
                ProductId = productVariantOption.ProductVariant.Product.PublicId,
                ProductVariantOptionId = productVariantOption.PublicId,
                ProductName = productVariantOption.ProductVariant.Product.Name,
                VariantName = productVariantOption.ProductVariant.Name,
                OptionName = productVariantOption.Name,
                MainImageUrl = productVariantOption.ImageUrl,
                Quantity = cartItem.Quantity,
                Price = productVariantOption.ProductVariant.Price,
                SalePrice = productVariantOption.ProductVariant.SalePrice,
                Discount = cartItem.Discount,
                TotalPrice = productVariantOption.ProductVariant.Price * cartItem.Quantity,
                Stock = productVariantOption.Stock
            };
        }
    }
}
