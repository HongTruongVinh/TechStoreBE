using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Action
{
    public class ActionModel
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime Time { get; set; }
    }

}
