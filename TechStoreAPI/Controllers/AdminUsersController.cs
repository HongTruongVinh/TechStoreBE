using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Constants;
using TechStore.Common.Models;
using TechStore.Model.DTOs.User;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

namespace TechStoreAPI.Controllers
{
    [Route(RouterControllerName.AdminUsers)]
    [ApiController]
    public class AdminUsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminUsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<UserListItemResponseModel>>>> GetAllUser()
        {
            var serviceResult = await _userService.GetAllUser();

            return serviceResult.ToActionResult(this);
        }

        [HttpGet("customers")]
        public async Task<ActionResult<ApiResponse<List<CustomerListItemModel>>>> GetCustomers()
        {
            var serviceResult = await _userService.GetCustomers();

            return serviceResult.ToActionResult(this);
        }

        [HttpGet("staffs")]
        public async Task<ActionResult<ApiResponse<List<StaffListItemModel>>>> GetStaffs()
        {
            var serviceResult = await _userService.GetStaffs();

            return serviceResult.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserResponseModel>>> GetUser(string id)
        {
            var serviceResult = await _userService.GetById(id);

            return serviceResult.ToActionResult(this);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateUser(string id, UserUpdateModel model)
        {
            var serviceResult = await _userService.UpdateUserInformation(id, model);

            return serviceResult.ToActionResult(this);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(string id)
        {
            var serviceResult = await _userService.DeleteUser(id);

            return serviceResult.ToActionResult(this);
        }
    }
}
