using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Statistic;
using TechStore.Service.Interfaces;

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
        public async Task<ApiResponse<DashboardOverviewModel>> GetStatisticsOverviewData()
        {
            var serviceResult = await _statisticsService.GetDashboardOverviewData();

            if (serviceResult.IsSuccess)
            {
                return new()
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new()
                {
                    PartnerCode = Messenger.NoExitData,
                    RetCode = ERetCode.NoExitData,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
        }
    }
}
