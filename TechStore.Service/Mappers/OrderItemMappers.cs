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
        public static OrderItemResponseModel ToOrderItemResponseModel(this OrderItem orderItem, Product product, Category category)
        {
            return new OrderItemResponseModel
            {
                OrderItemId = orderItem.PublicId,
                OrderId = orderItem.Order.PublicId,
                ProductId = product.PublicId,
                ProductName = product.Name,
                CategoryName = category.Name,
                MainImageUrl = product.MainImageUrl,
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
                list.Add(ToOrderItemResponseModel(orderItem, orderItem.Product, orderItem.Product.Category));
            }

            return list;
        }


    }
}
