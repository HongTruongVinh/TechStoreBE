using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Shipper
{
    public class ShipperCreateModel
    {
        public string Name { get; set; } = null!;         // Tên hãng vận chuyển

        public string? Description { get; set; }

        public required string Website { get; set; }

        public required string SupportPhone { get; set; }

        public required string LogoUrl { get; set; }

        public required bool IsActive { get; set; } = true;
    }
}
