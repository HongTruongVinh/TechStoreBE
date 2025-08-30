using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechStore.Service.Implementations;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitialDataController : ControllerBase
    {
        private readonly InitialDataService _service;

        public InitialDataController(InitialDataService service)
        {
            _service = service;
        }

        [HttpGet("Data")]
        public async Task<JsonResult> Get()
        {
            var result = await _service.GetAllInitData();
            return result;
        }

        [HttpGet("seed")]
        public async Task<string> CreateData()
        {
            var result = await _service.InitData();
            return result;
        }

        [HttpGet("delete")]
        public async Task<string> DeleteData()
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
        public async Task<string> ResetData()
        {
            return await _service.ResetData();
        }
    }
}
