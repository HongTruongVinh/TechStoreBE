using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Constants;
using TechStore.Model.DTOs.Statistic;
using TechStore.Common.Models;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

namespace TechStoreAPI.Controllers
{
    [Route(RouterControllerName.AdminStatistics)]
    [ApiController]
    public class AdminStatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public AdminStatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("overview")]
        public async Task<ActionResult<ApiResponse<DashboardOverviewModel>>> GetStatisticsOverviewData()
        {
            var serviceResult = await _statisticsService.GetDashboardOverviewData();

            return serviceResult.ToActionResult(this);
        }
    }
}
