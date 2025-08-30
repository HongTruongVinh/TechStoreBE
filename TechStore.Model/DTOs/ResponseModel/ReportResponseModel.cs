using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.ResponseModel
{
    public class ReportResponseModel
    {
        public required string ReportId { get; set; }

        public required string ReportType { get; set; }

        public required string Data { get; set; }
    }
}
