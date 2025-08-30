using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Shipper
{
    public class ShipperResponseModel
    {
        public required string ShipperId { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public string? Website { get; set; }

        public string? SupportPhone { get; set; }

        public string? LogoUrl { get; set; }

        public required bool IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
