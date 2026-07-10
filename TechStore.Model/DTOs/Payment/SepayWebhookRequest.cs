using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Payment
{
    public class SepayWebhookRequest
    {
        public required string Gateway { get; set; }
        public required string TransactionDate { get; set; }
        public required string AccountNumber { get; set; }

        public required string SubAccount { get; set; }
        public required string Code { get; set; }
        public required string Content { get; set; }

        public required string Description { get; set; }
        public decimal TransferAmount { get; set; }
        public required string ReferenceCode { get; set; }

        public decimal Accumulated { get; set; }
        public long Id { get; set; }
    }
}
