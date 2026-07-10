using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;
using TechStore.Common.Helpers;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.Order;
using TechStore.Model.DTOs.Payment;
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
                TotalPrice = order.TotalPrice,
                DiscountAmount = order.DiscountAmount,

                Items = order.OrderItems.Select(i => i.ToOrderItemResponseModel()).ToList(),

                ShippingCharge = order.ShippingCharge,
                FinalAmount = order.FinalAmount,
                UpdatedAt = order.UpdatedAt ?? DateTime.UtcNow,
                CreatedAt = order.CreatedAt,
            };
        }

        public static OrderHistoryModel ToOrderHistoryModel(this Order order)
        {
            return new OrderHistoryModel
            {
                OrderId = order.PublicId,
                FinalAmount = order.FinalAmount,
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

        public static InStoreOrderResponseModel ToInStoreOrderResponseModel(this Order order, List<OrderItem> orderItems, QRCode? paymentQROCde, Payment payment)
        {
            var model = new InStoreOrderResponseModel
            {
                Id = order.PublicId,
                CustomerName = order.CustomerName,
                CustomerPhonenumber = order.CustomerPhoneNumber,
                CustomerEmail = order.CustomerEmail ?? "",

                TotalPrice = order.TotalPrice,
                DiscountAmount = order.DiscountAmount,
                FinalAmount = order.FinalAmount,
                Items = orderItems.ToListOrderItemResponseModels(),

                Status = order.OrderStatus,
                PaymentQRCode = paymentQROCde != null ? Convert.ToBase64String(paymentQROCde.ImageData) : "",
                //PaymentId = order.Payment?.PublicId ?? "",
                PaymentId = "",
                PaymentStatus = payment.PaymentStatus,
                PaymentMethod = payment.PaymentMethod == EPaymentMethod.Cash? "Tiền mặt" : "Online",
                TransactionCode = payment.PaymentCode,

                InvoiceId = order.Invoice?.PublicId ?? "",
                Note = order.Note ?? "",
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt ?? DateTime.UtcNow
            };

            return model;
        }

        //public static OrderResponseModel ToOrderResponseModel(this Order order, User customer)
        //{
        //    var customerId = customer != null ? customer.PublicId : order.Customer != null ? order.Customer.PublicId : "";

        //    var orderResponseModel = new OrderResponseModel
        //    {
        //        Id = order.PublicId,
        //        CustomerId = customerId,
        //        CustomerName = order.CustomerName ?? "",
        //        ShippingAddress = order.ShippingAddress ?? "",
        //        CustomerPhonenumber = order.CustomerPhoneNumber ?? "",
        //        CustomerEmail = order.CustomerEmail ?? "",
        //        TotalPrice = order.TotalPrice,
        //        DiscountAmount = order.DiscountAmount,
        //        ShippingCharge = order.ShippingCharge,
        //        FinalAmount = order.FinalAmount,
        //        OrderStatus = order.OrderStatus,
        //        CreatedAt = TimeZoneHelper.ConvertUtcToGmt7(order.CreatedAt),
        //        UpdatedAt = TimeZoneHelper.ConvertUtcToGmt7(order.UpdatedAt ?? TimeZoneHelper.GetGmt7Now()),
        //        Items = new List<OrderItemResponseModel>()
        //    };

        //    foreach (var orderItem in order.OrderItems)
        //    {
        //        var product = orderItem.ProductVariantOption.ProductVariant.Product;
        //        var category = orderItem.ProductVariantOption.ProductVariant.Product.Category;

        //        var orderItemResponeModel = new OrderItemResponseModel
        //        {
        //            //ProductVariantOptionId = product.PublicId,
        //            ProductVariantOptionId = orderItem.ProductVariantOptionPublicId,
        //            OrderId = order.PublicId,
        //            Quantity = orderItem.Quantity,
        //            PriceAtOrderTime = orderItem.PriceAtOrderTime,
        //            TotalPrice = orderItem.TotalPrice,
        //            ProductName = product.Name,
        //            OptionName = orderItem.ProductVariantOption.Name,
        //            VariantName = orderItem.ProductVariantOption.ProductVariant.Name,
        //            CategoryName = category.Name,
        //            ImageUrl = product.MainImageUrl,
        //        };

        //        orderResponseModel.Items.Add(orderItemResponeModel);
        //    }

        //    if (order.ShippingDetail != null)
        //    {
        //        var shipper = order.ShippingDetail.Shipper;

        //        orderResponseModel.ShippingDetailId = shipper.PublicId;
        //        orderResponseModel.ShipperName = shipper.Name;
        //        orderResponseModel.TrackingNumber = order.ShippingDetail.TrackingNumber;
        //        orderResponseModel.ShippedDate = order.ShippingDetail.ShippedDate;
        //        orderResponseModel.EstimatedArrival = order.ShippingDetail.EstimatedArrival;
        //        orderResponseModel.ShippingNote = order.ShippingDetail.ShippingNote;
        //        orderResponseModel.FailureCount = order.ShippingDetail.FailureCount;
        //    }

        //    return orderResponseModel;
        //}
    
        public static OrderDetailResponseModel ToOrderDetailModel(this Order order)
        {
            ////////////////////////////////////////////////////////////////////////////////
            var orderResponseModel = new OrderDetailResponseModel
            {
                Id = order.PublicId,
                CustomerId = order.CustomerPublicId,
                CustomerName = order.CustomerName,
                ShippingAddress = order.ShippingAddress ?? "",
                CustomerPhonenumber = order.CustomerPhoneNumber,
                CustomerEmail = order.CustomerEmail ?? "",
                TotalPrice = order.TotalPrice,
                DiscountAmount = order.DiscountAmount,
                ShippingCharge = order.ShippingCharge,
                FinalAmount = order.FinalAmount,
                OrderStatus = order.OrderStatus,
                Notes = order.Note??"",
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
