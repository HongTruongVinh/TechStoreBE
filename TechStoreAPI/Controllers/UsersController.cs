using Microsoft.AspNetCore.Mvc;
using TechStore.Model.DTOs.User;
using TechStore.Common.Models;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<ApiResponse<UserResponseModel>>> GetUser(string userId)
        {
            var serviceResult = await _userService.GetById(userId);

            return serviceResult.ToActionResult(this);
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateProfile(UserUpdateModel model)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _userService.UpdateUserInformation(userId, model);

            return serviceResult.ToActionResult(this);
        }

        [HttpPut("update-information/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> AdminUpdateUser(string id, UserUpdateModel model)
        {
            var serviceResult = await _userService.UpdateUserInformation(id, model);

            return serviceResult.ToActionResult(this);
        }
    }
}
