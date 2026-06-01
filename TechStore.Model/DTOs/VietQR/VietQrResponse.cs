using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.VietQR
{
    public class VietQrResponse
    {
        public VietQrData? Data { get; set; }
    }

    public class VietQrData
    {
        public string QrDataURL { get; set; } = null!;
    }
}
