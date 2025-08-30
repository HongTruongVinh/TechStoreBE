using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Statistic;

namespace TechStore.Service.Interfaces
{
    public interface IStatisticsService
    {
        Task<ServiceResult<DashboardOverviewModel>> GetDashboardOverviewData();
    }
}
