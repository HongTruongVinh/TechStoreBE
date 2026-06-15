using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Payment
{
    public class CashPaymentCreateModel
    {
        public required string CustomerId { get; set; } //customerId
        public required string InvoiceId { get; set; }
        public required decimal Amount { get; set; }
        
    }
}
