using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStoreModel.Model.Invoice
{
    public class InvoiceResponseModel
    {
        public required string Id { get; set; }
        public required string OrderId { get; set; }

        public required string CustomerName { get; set; }
        public required string CustomerPhoneNumber { get; set; }

        public required decimal TotalPrice { get; set; } = 0;
        public required decimal DiscountAmount { get; set; }
        public required decimal FinalAmount { get; set; }

        public required EOrderType OrderType { get; set; }
        public required EInvoiceStatus InvoiceStatus { get; set; }

        public string? CashierName { get; set; }
        public DateTime? PaidAt { get; set; }


        public required DateTime CreatedAt { get; set; }
    }
}
