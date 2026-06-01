using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Data.Entities
{
    public class SearchKeyword : BaseEntity
    {
        public string? UserId { get; set; }
        public string? SessionId { get; set; }
        public required string Keyword { get; set; }
        public required string NormalizedKeyword { get; set; }
        public required DateTime SearchTime { get; set; }
        public required int ResultCount { get; set; }
        public Guid? ClickedProductId { get; set; }
        public required EDeviceType DeviceType { get; set; }
        public string? IpAddress { get; set; }
        public bool IsSuccessful { get; set; }

    }
}
