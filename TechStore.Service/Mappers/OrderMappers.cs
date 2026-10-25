
using TechStore.Common.Helpers;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.Order;
using TechStore.Model.DTOs.User;

namespace TechStore.Service.Mappers
{
    public static class OrderMappers
    {

        public static ListItemOrderModel ToListItemOrderModel(this Order order)
        {
            return new ListItemOrderModel
            {
                Id = order.PublicId,
                CustomerId = order.CustomerPublicId,
                CustomerName = order.CustomerName,

                CustomerPhonenumber = order.CustomerPhoneNumber,
                CustomerEmail = order.CustomerEmail ?? "",
                OrderStatus = order.OrderStatus,

                ShippingAddress = order.ShippingAddress ?? "",
                TotalPrice = order.SubtotalAmount,
                DiscountAmount = order.DiscountAmount,

                Items = order.OrderItems.Select(i => i.ToOrderItemResponseModel()).ToList(),

                ShippingCharge = order.ShippingCharge,
                FinalAmount = order.TotalAmount,
                UpdatedAt = order.UpdatedAt ?? DateTime.UtcNow,
                CreatedAt = order.CreatedAt,
            };
        }

        public static OrderHistoryModel ToOrderHistoryModel(this Order order)
        {
            return new OrderHistoryModel
            {
                OrderId = order.PublicId,
                FinalAmount = order.TotalAmount,
                OrderDate = order.CreatedAt 
            };
        }

        public static List<OrderHistoryModel> ToListOrdersHistoryModel(this List<Order> orders)
        {
            var list = new List<OrderHistoryModel>();
            foreach (var order in orders)
            {
                list.Add(ToOrderHistoryModel(order));
            }

            return list;
        }
    
        public static OrderDetailResponseModel ToOrderDetailModel(this Order order)
        {
            var orderResponseModel = new OrderDetailResponseModel
            {
                Id = order.PublicId,
                CustomerId = order.CustomerPublicId,
                CustomerName = order.CustomerName,
                ShippingAddress = order.ShippingAddress ?? "",
                CustomerPhonenumber = order.CustomerPhoneNumber,
                CustomerEmail = order.CustomerEmail ?? "",
                TotalPrice = order.SubtotalAmount,
                DiscountAmount = order.DiscountAmount,
                ShippingCharge = order.ShippingCharge,
                FinalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                Notes = order.Note,
                CreatedAt = TimeZoneHelper.ConvertUtcToGmt7(order.CreatedAt),
                UpdatedAt = TimeZoneHelper.ConvertUtcToGmt7(order.UpdatedAt ?? TimeZoneHelper.GetGmt7Now()),
                Items = order.OrderItems.ToList().ToListOrderItemResponseModels(),
                Invoice = order.Invoice != null ? order.Invoice.ToInvoiceModel(order) : null,
                ShippingDetail = order.ShippingDetail != null ? order.ShippingDetail.ToResponseModel() : null
            };

            return orderResponseModel;
        }

    }
}
