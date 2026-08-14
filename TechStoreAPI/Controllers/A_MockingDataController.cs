using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechStore.Service.Implementations;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class A_MockingDataController : ControllerBase
    {
        private readonly MockingDataService _service;

        public A_MockingDataController(MockingDataService service)
        {
            _service = service;
        }

        [HttpGet("Data")]
        public async Task<ActionResult<JsonResult>> GetData()
        {
            var result = await _service.GetAllInitData();
            return result;
        }

        [HttpGet("seed")]
        public async Task<ActionResult<string>> AddData()
        {
            var result = await _service.InitData();
            return result;
        }

        [HttpGet("delete")]
        public async Task<ActionResult<string>> DeleteData()
        {
            var result = await _service.DeleteAllInitData();

            if (result)
            {
                return "Xoa thanh cong";
            }
            else
            {
                return "Xoa KHONG thanh cong";
            }
        }

        [HttpGet("reset")]
        public async Task<ActionResult<string>> ResetData()
        {
            return await _service.ResetData();
        }
    }
}
