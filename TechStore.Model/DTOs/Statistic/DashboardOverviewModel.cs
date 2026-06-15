using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Model.DTOs.Action;
using TechStore.Model.DTOs.Product;
using TechStore.Model.DTOs.User;
namespace TechStore.Model.DTOs.Statistic
{
    public class DashboardOverviewModel
    {
        public required RadialBarChartData ProcessingOfPendingOrders { get; set; }

        public required RadialBarChartData DeliveringOfProcessingOrders { get; set; }

        public required RadialBarChartData ComplatedOfDeliveringOrders { get; set; }

        public required List<ChartData> CategoryChartData { get; set; }

        public required List<ChartDecimalData> TotalRevenueChartData { get; set; }

        public required List<ChartDecimalData> TotalProfitChartData { get; set; }

        public required List<ChartData> TotalOrdersChartData { get; set; }

        public required List<AdminListItemProductModel> HotProducts { get; set; }

        public required List<AdminListItemProductModel> TopSoldProducts { get; set; }

        public required List<AdminListItemProductModel> TopRatedProducts { get; set; }

        public required List<UserListItemResponseModel> LoyalCustomer { get; set; }
        public required List<ActionModel> RecentlyActions { get; set; }
    }
}
