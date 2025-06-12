using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class Shipper : BaseEntity
    {
        public required string Name { get; set; }         // Tên hãng vận chuyển

        public string? Description { get; set; }

        public string? Website { get; set; }

        public string? SupportPhone { get; set; }

        public string? LogoUrl { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
