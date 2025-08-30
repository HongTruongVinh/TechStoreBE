using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.Invoice;
using TechStoreModel.Model.Invoice;

namespace TechStore.Service.Mappers
{
    public static class InvoiceMappers
    {
        public static InvoiceListItemModel ToInvoiceListItemModel(this Invoice invoice, Order order)
        {
            return new InvoiceListItemModel
            {
                InvoiceId = invoice.PublicId,
                OrderId = order.PublicId,

                CustomerName = order.CustomerName,
                CustomerPhoneNumber = order.CustomerPhoneNumber,

                TotalPrice = invoice.TotalPrice,
                DiscountAmount = invoice.DiscountAmount,
                FinalAmount = invoice.FinalAmount,

                InvoiceStatus = invoice.InvoiceStatus,
                OrderType = order.OrderType,

                PaidAt = invoice.PaidAt,
                CashierName = invoice.CashierName,

                CreatedAt = invoice.CreatedAt
            };
        }

        public static InvoiceResponseModel ToInvoiceModel(this Invoice invoice, Order order)
        {
            return new InvoiceResponseModel
            {
                InvoiceId = invoice.PublicId,
                OrderId = order.PublicId,

                CustomerName= order.CustomerName,
                CustomerPhoneNumber = order.CustomerPhoneNumber,

                TotalPrice = invoice.TotalPrice,
                DiscountAmount = invoice.DiscountAmount,
                FinalAmount = invoice.FinalAmount,

                InvoiceStatus = invoice.InvoiceStatus,
                OrderType = order.OrderType,

                PaidAt = invoice.PaidAt,
                CashierName = invoice.CashierName,

                CreatedAt = invoice.CreatedAt
            };
        }
    }
}
