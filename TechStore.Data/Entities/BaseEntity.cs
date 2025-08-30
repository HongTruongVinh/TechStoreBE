using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Data.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }// Internal ID (Primary Key)
        public required string PublicId { get; set; } // Public ID (display hoặc dùng cho API, không là khóa chính)


        public required EEntityStatus EntityStatus { get; set; }

        public required DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
}
