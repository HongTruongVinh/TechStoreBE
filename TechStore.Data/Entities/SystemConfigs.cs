using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class SystemConfigs
    {
        public Guid Id { get; set; }
        public required bool IsShowImportantNotification { get; set; }
        public required bool isAiChatbotEnabled { get; set; }
    }
}
