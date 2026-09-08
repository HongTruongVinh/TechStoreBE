using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class AiConversation : BaseEntity
    {
        public Guid? UserId { get; set; }
        public string? UserPublicId { get; set; }

        public string? GuestId { get; set; }

        public string? LastInteractionId { get; set; }

        public string? Title { get; set; }
    }
}
