using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Statistic
{
    public class ChartData
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    public class ChartDecimalData
    {
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }
    }

    public class RadialBarChartData
    {
        public int Goal { get; set; }
        public int Progress { get; set; }
        public long ProgressPercent { get; set; }
    }
}
