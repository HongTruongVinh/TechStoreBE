using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Extensions;
using TechStore.Data.Entities;

namespace TechStore.Service.Mappers
{
    public static class PaymentSnapshotItemMappers
    {
        public static OrderItem ToOrderItem(this PaymentSnapshotItem snapshotItem, Guid orderId)
        {
            return new OrderItem
            {
                ProductVariantOptionId = snapshotItem.ProductVariantOptionId,
                OrderId = orderId,
                ProductVariantOptionPublicId = snapshotItem.ProductVariantOptionPublicId,
                CategoryName = snapshotItem.CategoryName,
                ProductName = snapshotItem.ProductName,
                ImageUrl = snapshotItem.UrlImage,
                Quantity = snapshotItem.Quantity,
                PriceAtOrderTime = snapshotItem.PriceAtOrderTime,
                TotalPrice = snapshotItem.TotalPrice,

                PublicId = ShareFunctions.GenarateRandomStringId(),
                CreatedAt = snapshotItem.CreatedAt,
            };
        }
    }
}
