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
        public static ListItemInvoiceModel ToListItemInvoiceModel(this Invoice invoice, Order order)
        {
            return new ListItemInvoiceModel
            {
                Id = invoice.PublicId,
                OrderId = order.PublicId,

                CustomerName = order.CustomerName,
                CustomerPhoneNumber = order.CustomerPhoneNumber,

                TotalPrice = invoice.TotalAmount,
                DiscountAmount = invoice.PaidAmount,
                FinalAmount = invoice.RemainingAmount,

                InvoiceStatus = invoice.InvoiceStatus,

                PaidAt = invoice.PaidAt,

                CreatedAt = invoice.CreatedAt
            };
        }

        public static InvoiceResponseModel ToInvoiceModel(this Invoice invoice, Order order)
        {
            return new InvoiceResponseModel
            {
                Id = invoice.PublicId,
                OrderId = order.PublicId,

                CustomerName= order.CustomerName,
                CustomerPhoneNumber = order.CustomerPhoneNumber,

                TotalPrice = invoice.TotalAmount,
                PaidAmount = invoice.PaidAmount,
                RemainingAmount = invoice.RemainingAmount,
                InvoiceStatus = invoice.InvoiceStatus,

                Payments = invoice.Payments.Select(p => p.ToPaymentResponseModel()).ToList(),
                PaidAt = invoice.PaidAt,
                CreatedAt = invoice.CreatedAt
            };
        }
    }
}
