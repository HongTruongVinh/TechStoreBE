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
        public static CartItemResponseModel ToCartItemResponseModel(this CartItem cartItem, Product product)
        {
            return new CartItemResponseModel
            {
                CartItemId = cartItem.PublicId,
                CartId = cartItem.PublicId,
                ProductId = product.PublicId,
                ProductName = product.Name,
                MainImageUrl = product.MainImageUrl,
                Quantity = cartItem.Quantity,
                PriceAtOrderTime = product.Price,
                Discount = cartItem.Discount,
                TotalPrice = cartItem.TotalPrice
            };
        }
    }
}
