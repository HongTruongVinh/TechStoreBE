using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class ProductVariant : BaseEntity
    {
        public required Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        // Ví dụ: 6GB 128GB, 8GB 256GB, 12GB 512GB
        public required string Name { get; set; }
        public required string Description { get; set; }

        // Performance
        public string? Cpu { get; set; }
        public string? Gpu { get; set; }
        public int? Ram { get; set; }
        public int? Storage { get; set; }

        // Display
        public decimal? ScreenSize { get; set; }
        public string? ScreenResolution { get; set; }
        public int? RefreshRate { get; set; }
        public string? PanelType { get; set; }
        public int? ScreenBrightness { get; set; }

        // Battery
        public int? BatteryCapacity { get; set; }
        public int? ChargingWattage { get; set; }

        // Camera
        public string? MainCamera { get; set; }
        public string? FrontCamera { get; set; }

        // Connectivity
        public string? Wifi { get; set; }
        public string? Bluetooth { get; set; }

        // Physical
        public decimal? Weight { get; set; }
        public string? Dimensions { get; set; }


        public required decimal Price { get; set; }
        public required decimal ImportPrice { get; set; }
        public int SoldCount { get; set; }


        public ICollection<ProductVariantOption> Options { get; set; } = new List<ProductVariantOption>();
    }
}
