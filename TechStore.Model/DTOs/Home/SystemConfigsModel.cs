using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Home
{
    public class SystemConfigsModel
    {
        public required bool IsShowImportantNotification { get; set; }
        public required bool isAiChatbotEnabled { get; set; }
    }
}
