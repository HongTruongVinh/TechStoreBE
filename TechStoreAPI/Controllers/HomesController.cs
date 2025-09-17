using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Home;
using TechStore.Service.Interfaces;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomesController : ControllerBase
    {
        private readonly IHomeService _homeService;
        public HomesController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        [HttpGet]
        public async Task<ApiResponse<HomeResponseModel>> GetData()
        {
            var serviceResult = await _homeService.GetHomeProduct();

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
