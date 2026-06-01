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
        //public static OrderDetailResponseModel ToOrderDetailModelAsync(this Order order, 
        //    QRCode? qrCode, Invoice? invoice, Payment? payment, ShippingDetail? shippingDetail, Shipper? shipper)
        //{
        //    OrderDetailResponseModel orderDetailResponseModel = new OrderDetailResponseModel
        //    {
        //        OrderId = order.PublicId,
        //        CustomerId = order.Customer?.PublicId ?? "",
        //        CustomerName = order.CustomerName,
        //        ShippingAddress = order.ShippingAddress ?? "",
        //        CustomerPhonenumber = order.CustomerPhoneNumber,
        //        CustomerEmail = order.CustomerEmail ?? "",
        //        TotalPrice = order.TotalPrice,
        //        ShippingCharge = order.ShippingCharge,
        //        DiscountAmount = order.DiscountAmount,
        //        FinalAmount = order.FinalAmount,
        //        PaymentMethod = order.PaymentMethod,
        //        OrderStatus = order.OrderStatus,
        //        Payments = new List<PaymentResponseModel>(),
        //        CreatedAt = TimeZoneHelper.ConvertUtcToGmt7(order.CreatedAt),
        //        UpdatedAt = TimeZoneHelper.ConvertUtcToGmt7(order.UpdatedAt ?? TimeZoneHelper.GetGmt7Now()),
        //        Items = order.OrderItems.ToList().ToListOrderItemResponseModels(),

        //    };

        //    if (qrCode != null)
        //    {
        //        orderDetailResponseModel.QRCode = Convert.ToBase64String(qrCode.ImageData);
        //    }

        //    if (invoice != null)
        //    {
        //        orderDetailResponseModel.Invoice = invoice.ToInvoiceModel(order);

        //        if (payment != null)
        //        {
        //            orderDetailResponseModel.Payment = payment.ToPaymentResponseModel(order, qrCode);
        //        }

        //        if (shippingDetail != null && shipper != null)
        //        {
        //            orderDetailResponseModel.ShippingDetailId = shipper.PublicId;
        //            orderDetailResponseModel.ShipperName = shipper.Name;
        //            orderDetailResponseModel.TrackingNumber = shippingDetail.TrackingNumber;
        //            orderDetailResponseModel.ShippedDate = shippingDetail.ShippedDate;
        //            orderDetailResponseModel.EstimatedArrival = shippingDetail.EstimatedArrival;
        //            orderDetailResponseModel.ShippingNote = shippingDetail.ShippingNote;
        //            orderDetailResponseModel.FailureCount = shippingDetail.FailureCount;
        //        }
        //    }

        //    return orderDetailResponseModel;
        //}

        public static OrderListItemModel ToOrderListItemModel(this Order order)
        {
            return new OrderListItemModel
            {
                Id = order.PublicId,
                CustomerId = order.Customer?.PublicId ?? "",
                CustomerName = order.CustomerName,
                CustomerPhonenumber = order.CustomerPhoneNumber,
                CustomerEmail = order.CustomerEmail ?? "",
                OrderStatus = order.OrderStatus,
                ShippingAddress = order.ShippingAddress ?? "",
                TotalPrice = order.TotalPrice,
                DiscountAmount = order.DiscountAmount,
                ShippingCharge = order.ShippingCharge,
                FinalAmount = order.FinalAmount,
                PaymentMethod = order.PaymentMethod,
                OrderType = order.OrderType,
                UpdatedAt = order.UpdatedAt ?? DateTime.UtcNow,
                CreatedAt = order.CreatedAt,
                OrderItems = order.OrderItems.ToList().ToListOrderItemResponseModels()
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
                OrderId = order.PublicId,
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
                TransactionCode = payment.TransactionCode,

                InvoiceId = order.Invoice?.PublicId ?? "",
                Note = order.Note ?? "",
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt ?? DateTime.UtcNow
            };

            return model;
        }

        public static OrderResponseModel ToOrderResponseModel(
            this Order order, 
            User customer)
        {
            var customerId = customer != null ? customer.PublicId : order.Customer != null ? order.Customer.PublicId : "";

            var orderResponseModel = new OrderResponseModel
            {
                Id = order.PublicId,
                CustomerId = customerId,
                CustomerName = order.CustomerName ?? "",
                ShippingAddress = order.ShippingAddress ?? "",
                CustomerPhonenumber = order.CustomerPhoneNumber ?? "",
                CustomerEmail = order.CustomerEmail ?? "",
                TotalPrice = order.TotalPrice,
                DiscountAmount = order.DiscountAmount,
                ShippingCharge = order.ShippingCharge,
                FinalAmount = order.FinalAmount,
                PaymentMethod = order.PaymentMethod,
                OrderType = order.OrderType,
                OrderStatus = order.OrderStatus,
                CreatedAt = TimeZoneHelper.ConvertUtcToGmt7(order.CreatedAt),
                UpdatedAt = TimeZoneHelper.ConvertUtcToGmt7(order.UpdatedAt ?? TimeZoneHelper.GetGmt7Now()),
                Items = new List<OrderItemResponseModel>()
            };

            foreach (var orderItem in order.OrderItems)
            {
                var product = orderItem.ProductVariantOption.ProductVariant.Product;
                var category = orderItem.ProductVariantOption.ProductVariant.Product.Category;

                var orderItemResponeModel = new OrderItemResponseModel
                {
                    //ProductVariantOptionId = product.PublicId,
                    ProductVariantOptionId = orderItem.ProductVariantOptionPublicId,
                    OrderId = order.PublicId,
                    Quantity = orderItem.Quantity,
                    PriceAtOrderTime = orderItem.PriceAtOrderTime,
                    Discount = orderItem.Discount,
                    TotalPrice = orderItem.TotalPrice,
                    ProductName = product.Name,
                    OptionName = orderItem.ProductVariantOption.Name,
                    VariantName = orderItem.ProductVariantOption.ProductVariant.Name,
                    CategoryName = category.Name,
                    MainImageUrl = product.MainImageUrl,
                };

                orderResponseModel.Items.Add(orderItemResponeModel);
            }

            if (order.ShippingDetail != null)
            {
                var shipper = order.ShippingDetail.Shipper;

                orderResponseModel.ShippingDetailId = shipper.PublicId;
                orderResponseModel.ShipperName = shipper.Name;
                orderResponseModel.TrackingNumber = order.ShippingDetail.TrackingNumber;
                orderResponseModel.ShippedDate = order.ShippingDetail.ShippedDate;
                orderResponseModel.EstimatedArrival = order.ShippingDetail.EstimatedArrival;
                orderResponseModel.ShippingNote = order.ShippingDetail.ShippingNote;
                orderResponseModel.FailureCount = order.ShippingDetail.FailureCount;
            }

            return orderResponseModel;
        }
    
        public static OrderDetailResponseModel ToOrderDetailModel(
            this Order order, 
            User? customer = null
            )
        {
            var customerId = customer != null ? customer.PublicId : order.Customer != null ? order.Customer.PublicId : "";

            var orderResponseModel = new OrderDetailResponseModel
            {
                OrderId = order.PublicId,
                CustomerId = customerId,
                CustomerName = order.CustomerName ?? "",
                ShippingAddress = order.ShippingAddress ?? "",
                CustomerPhonenumber = order.CustomerPhoneNumber ?? "",
                CustomerEmail = order.CustomerEmail ?? "",
                TotalPrice = order.TotalPrice,
                DiscountAmount = order.DiscountAmount,
                ShippingCharge = order.ShippingCharge,
                FinalAmount = order.FinalAmount,
                PaymentMethod = order.PaymentMethod,
                OrderStatus = order.OrderStatus,
                Notes = order.Note,
                Payments = new List<PaymentResponseModel>(),
                CreatedAt = TimeZoneHelper.ConvertUtcToGmt7(order.CreatedAt),
                UpdatedAt = TimeZoneHelper.ConvertUtcToGmt7(order.UpdatedAt ?? TimeZoneHelper.GetGmt7Now()),
                Items = new List<OrderItemResponseModel>()
            };

            var orderItems = order.OrderItems.ToList();
            var shippingDetail = order.ShippingDetail;
            var orderTrackingQR = order.QRCode;
            var invoice = order.Invoice;
            var payments = order.Payments;

            foreach (var orderItem in orderItems)
            {
                var product = orderItem.ProductVariantOption.ProductVariant.Product;
                var category = orderItem.ProductVariantOption.ProductVariant.Product.Category;

                var orderItemResponeModel = new OrderItemResponseModel
                {
                    ProductVariantOptionId = product.PublicId,
                    OrderId = order.PublicId,
                    Quantity = orderItem.Quantity,
                    PriceAtOrderTime = orderItem.PriceAtOrderTime,
                    Discount = orderItem.Discount,
                    TotalPrice = orderItem.TotalPrice,
                    ProductName = product.Name,
                    OptionName = orderItem.ProductVariantOption.Name,
                    VariantName = orderItem.ProductVariantOption.ProductVariant.Name,
                    CategoryName = category.Name,
                    MainImageUrl = orderItem.ProductVariantOption.ImageUrl,
                };

                orderResponseModel.Items.Add(orderItemResponeModel);
            }

            if (shippingDetail != null)
            {
                if (shippingDetail != null)
                {
                    var shipper = shippingDetail.Shipper;

                    orderResponseModel.ShippingDetailId = shipper.PublicId;
                    orderResponseModel.ShipperName = shipper.Name;
                    orderResponseModel.TrackingNumber = shippingDetail.TrackingNumber;
                    orderResponseModel.ShippedDate = shippingDetail.ShippedDate;
                    orderResponseModel.EstimatedArrival = shippingDetail.EstimatedArrival;
                    orderResponseModel.ShippingNote = shippingDetail.ShippingNote;
                    orderResponseModel.FailureCount = shippingDetail.FailureCount;
                }
            }

            if (orderTrackingQR != null)
            {
                orderResponseModel.QRCode = Convert.ToBase64String(orderTrackingQR.ImageData);
            }

            if (invoice != null)
            {
                orderResponseModel.Invoice = invoice.ToInvoiceModel(order);
            }

            //if (payment != null)
            //{
            //    orderResponseModel.Payment = payment.ToPaymentResponseModel(order, orderTrackingQR);
            //}

            foreach(var payment in payments)
            {
                orderResponseModel.Payments.Add(payment.ToPaymentResponseModel(order, orderTrackingQR));
            }

            return orderResponseModel;
        }
    }
}
