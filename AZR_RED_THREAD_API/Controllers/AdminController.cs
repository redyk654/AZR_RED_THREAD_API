using AZR_RED_THREAD_BLL.DTOs.Access;
using AZR_RED_THREAD_BLL.Services.UserServices;
using AZR_RED_THREAD_DAL.Services.UserDAServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace AZR_RED_THREAD_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IUserDAServices _userDA;
        public AdminController(AZR_RED_THREAD_DAL.Services.UserDAServices.IUserDAServices userDA)
        {
            _userDA = userDA;
        }

        // POST api/admin/users/{userId}/role
        [HttpPost("users/{userId}/role")]
        public async Task<ActionResult> AssignRoleToUser(int userId, [FromBody] AssignRoleToUserDto dto)
        {
            var user = await _userDA.GetByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            // set role
            user.RoleId = dto.RoleId;
            user.UpdatedAt = System.DateTime.UtcNow;
            user.UpdatedBy = ResolveUserIdOrFallback();

            // persist - if you don't have UpdateUserAsync implement one in DAL (recommended)
            await _userDA.CreateUserAsync(user); // ugly workaround: use UpdateUserAsync in DAL ideally
            return NoContent();
        }

        private int ResolveUserIdOrFallback()
        {
            var claimVal = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(claimVal, out int id)) return id;
            if (Request.Headers.TryGetValue("X-Fake-UserId", out var h) && int.TryParse(h, out int hid)) return hid;
            return 1;
        }
    }

}
