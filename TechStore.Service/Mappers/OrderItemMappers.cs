using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.Order;

namespace TechStore.Service.Mappers
{
    public static class OrderItemMappers
    {
        public static OrderItemResponseModel ToOrderItemResponseModel(this OrderItem orderItem)
        {
            return new OrderItemResponseModel
            {
                OrderItemId = orderItem.PublicId,
                OrderId = orderItem.Order.PublicId,
                ProductVariantOptionId = orderItem.ProductVariantOption.PublicId,
                ProductName = orderItem.ProductVariantOption.ProductVariant.Name,
                CategoryName = orderItem.ProductVariantOption.ProductVariant.Product.Category.Name,
                MainImageUrl = orderItem.ProductVariantOption.ImageUrl,
                PriceAtOrderTime = orderItem.PriceAtOrderTime,
                Discount = orderItem.Discount,
                Quantity = orderItem.Quantity,
                TotalPrice = orderItem.TotalPrice,
            };
        }

        public static List<OrderItemResponseModel> ToListOrderItemResponseModels(this List<OrderItem> orderItems)
        {
            var list = new List<OrderItemResponseModel>();

            foreach (var orderItem in orderItems)
            {
                list.Add(ToOrderItemResponseModel(orderItem));
            }

            return list;
        }


    }
}
