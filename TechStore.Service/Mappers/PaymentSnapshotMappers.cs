using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Extensions;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.Order;

namespace TechStore.Service.Mappers
{
    public static class PaymentSnapshotMappers
    {
        public static OrderDetailResponseModel ToOrderDetailResponseModel(this PaymentSnapshot snapshot, string customerPublicId)
        {
            return new OrderDetailResponseModel
            {
                Id = snapshot.PublicId,
                CustomerId = customerPublicId,
                CustomerName = snapshot.CustomerName,
                CustomerPhonenumber = snapshot.CustomerPhoneNumber,
                CustomerEmail = snapshot.CustomerEmail,
                ShippingAddress = snapshot.ShippingAddress,
                Notes = snapshot.Note,

                OrderStatus = snapshot.ExpiredAt > DateTime.UtcNow ? Common.Enums.EOrderStatus.Pending : Common.Enums.EOrderStatus.Canceled,

                TotalPrice = snapshot.SubtotalAmount,
                DiscountAmount = snapshot.DiscountAmount,
                ShippingCharge = snapshot.ShippingCharge,
                FinalAmount = snapshot.TotalAmount,
                Items = snapshot.Items.Select(x => x.ToOrderItemResponseModel()).ToList(),

                UpdatedAt = snapshot.UpdatedAt ?? DateTime.UtcNow,
                CreatedAt = snapshot.CreatedAt,

                ExpiredAt = snapshot.ExpiredAt,
            };
        }

        public static OrderItemResponseModel ToOrderItemResponseModel(this PaymentSnapshotItem snapshotItem)
        {
            return new OrderItemResponseModel
            {
                ProductVariantOptionId = snapshotItem.ProductVariantOptionPublicId,
                ProductName = snapshotItem.ProductName,
                CategoryName = snapshotItem.CategoryName,
                ImageUrl = snapshotItem.UrlImage,
                PriceAtOrderTime = snapshotItem.PriceAtOrderTime,
                Quantity = snapshotItem.Quantity,
                TotalPrice = snapshotItem.TotalPrice,
            };
        }

        public static ListItemOrderModel ToListItemOrderModel(this PaymentSnapshot snapshot, string customerPublicId)
        {
            return new ListItemOrderModel
            {
                Id = snapshot.PublicId,
                CustomerId = customerPublicId,
                CustomerName = snapshot.CustomerName,

                CustomerPhonenumber = snapshot.CustomerPhoneNumber,
                CustomerEmail = snapshot.CustomerEmail ?? "",
                OrderStatus = snapshot.ExpiredAt > DateTime.UtcNow ? Common.Enums.EOrderStatus.Pending : Common.Enums.EOrderStatus.Canceled,

                ShippingAddress = snapshot.ShippingAddress ?? "",
                TotalPrice = snapshot.SubtotalAmount,
                DiscountAmount = snapshot.DiscountAmount,

                Items = snapshot.Items.Select(i => i.ToOrderItemResponseModel()).ToList(),

                ShippingCharge = snapshot.ShippingCharge,
                FinalAmount = snapshot.TotalAmount,
                UpdatedAt = snapshot.UpdatedAt ?? DateTime.UtcNow,
                CreatedAt = snapshot.CreatedAt,

                ExpiredAt = snapshot.ExpiredAt,
            };
        }
    }
}
