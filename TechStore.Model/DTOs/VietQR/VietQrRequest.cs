using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.VietQR
{
    public class VietQrRequest
    {
        public required string AccountNo { get; set; }
        public required string AccountName { get; set; }
        public int AcqId { get; set; }
        public decimal Amount { get; set; }
        public required string AddInfo { get; set; }
    }
}
