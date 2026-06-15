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
                ProductVariantOptionId = orderItem.ProductVariantOptionPublicId,
                ProductName = orderItem.ProductName,
                CategoryName = orderItem.CategoryName,
                ImageUrl = orderItem.ImageUrl,
                PriceAtOrderTime = orderItem.PriceAtOrderTime,
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
