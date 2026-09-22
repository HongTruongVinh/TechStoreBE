using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Model.DTOs.ProductVariantOption;

namespace TechStore.Model.DTOs.ProductVariant
{
    public class ProductVariantCreateModel
    {
        public required string Name { get; set; }
        public required string Description { get; set; }

        public required decimal Price { get; set; }
        public required decimal ImportPrice { get; set; }

        // Performance
        public string? OperatingSystem { get; set; }
        public string? Cpu { get; set; }
        public string? Gpu { get; set; }
        public string? Ram { get; set; }
        public string? Storage { get; set; }
        public string? AvailableStorage { get; set; }

        // Display
        public string? ScreenSize { get; set; }
        public string? ScreenResolution { get; set; }
        public string? RefreshRate { get; set; }
        public string? PanelType { get; set; }
        public string? ScreenBrightness { get; set; }

        // Battery
        public string? BatteryCapacity { get; set; }
        public string? BatteryType { get; set; }
        public string? ChargingWattage { get; set; }
        public string? BetteryEngine { get; set; }

        // Camera
        public string? MainCamera { get; set; }
        public string? FrontCamera { get; set; }

        // Connectivity
        public string? Wifi { get; set; }
        public string? Bluetooth { get; set; }

        // Physical
        public decimal? Weight { get; set; }
        public string? Dimensions { get; set; }

        public required List<ProductVariantOptionCreateModel> Options { get; set; }

    }
}
