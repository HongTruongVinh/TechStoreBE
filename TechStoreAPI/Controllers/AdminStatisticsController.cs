//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Net;
//using TechStoreModel.Base;
//using TechStoreModel.Constants;
//using TechStoreModel.Enums;
//using TechStoreModel.Model.Statistic;
//using TechStoreModel.Model.ResponseModel;
//using TechStoreServices.IServices;
//using TechStoreServices.Services;

//namespace TechStoreAPI.Controllers
//{
//    [Route(RouterControllerName.AdminStatistics)]
//    [ApiController]
//    public class AdminStatisticsController : ControllerBase
//    {
//        private readonly IStatisticsService _statisticsService;

//        public AdminStatisticsController(IStatisticsService statisticsService)
//        {
//            _statisticsService = statisticsService;
//        }

//        [HttpGet("overview")]
//        public async Task<ApiResponse<DashboardOverviewModel>> GetStatisticsOverviewData()
//        {
//            var serviceResult = await _statisticsService.GetDashboardOverviewData();

//            if (serviceResult.IsSuccess)
//            {
//                return new()
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = serviceResult.Data,
//                    SystemMessage = serviceResult.Message,
//                    StatusCode = (int)HttpStatusCode.OK
//                };
//            }
//            else
//            {
//                return new()
//                {
//                    PartnerCode = Messenger.NoExitData,
//                    RetCode = ERetCode.NoExitData,
//                    Data = serviceResult.Data,
//                    SystemMessage = serviceResult.Message,
//                    StatusCode = (int)HttpStatusCode.OK
//                };
//            }
//        }
//    }
//}
