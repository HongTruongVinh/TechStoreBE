using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Common.Models
{
    public class ApiResponse<T>
    {
        public string? PartnerCode { get; set; }

        private int _code = 0;

        public ERetCode RetCode
        {
            get { return (ERetCode)_code; }
            set { _code = (int)value; }
        }

        public T? Data { get; set; }

        public int StatusCode { get; set; }

        public string? SystemMessage { get; set; }

        public ApiResponse(ERetCode retCode, T value)
        {
            this.RetCode = retCode;
            this.Data = value;
        }

        public ApiResponse()
        {
            PartnerCode = "0";
        }
    }
}
