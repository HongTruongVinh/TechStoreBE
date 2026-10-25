using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Payment
{
    public class SepayWebhookRequest
    {
        /// <summary>
        /// Ex: BIDV, VPBank, Vietcombank, Techcombank, ...
        /// </summary>
        public required string Gateway { get; set; }

        /// <summary>
        /// Ex: 2023-08-01 12:34:56
        /// </summary>
        public required string TransactionDate { get; set; }

        /// <summary>
        /// Ex: 1234567890
        /// </summary>
        public required string AccountNumber { get; set; }

        /// <summary>
        /// Ex: 1234512345        
        /// </summary>
        public required string SubAccount { get; set; }

        /// <summary>
        /// Ex: STORE20261019124041
        /// </summary>
        public required string Code { get; set; }

        /// <summary>
        /// Ex: 152459108061 0393574180 STORE20261019124041
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// Ex: EBankAPINotify 152459108061 0393574180 STORE20261019124041
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Ex: 8099
        /// </summary>
        public decimal TransferAmount { get; set; }

        /// <summary>
        /// Ex: 551538ee-0c99-12b5-b26d-c23456e2340d
        /// </summary>
        public required string ReferenceCode { get; set; }

        /// <summary>
        /// Ex: 0
        /// </summary>
        public decimal Accumulated { get; set; }

        /// <summary>
        /// The unique transaction ID assigned by SEPay.
        /// The same ID is retained when SEPay retries the webhook,
        /// allowing the transaction to be identified and duplicate
        /// webhook deliveries to be detected.
        /// Example: 91317123
        /// </summary>
        public long Id { get; set; }
    }
}
