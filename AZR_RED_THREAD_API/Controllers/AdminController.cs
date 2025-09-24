using AZR_RED_THREAD_BLL.DTOs.Access;
using AZR_RED_THREAD_BLL.DTOs.UserDto;
using AZR_RED_THREAD_BLL.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace AZR_RED_THREAD_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public AdminController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        // GET api/admin/users
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            // In production: add authorization check (only admin)
            var list = await _userServices.GetAllUsersAsync();
            return Ok(list);
        }

        // POST api/admin/users/{userId}/role
        [HttpPost("users/{userId}/role")]
        public async Task<ActionResult> AssignRoleToUser(int userId, [FromBody] AssignRoleToUserDto dto)
        {
            // optionally validate dto.UserId == userId
            var performedBy = ResolveUserIdOrFallback();
            await _userServices.AssignRoleToUserAsync(userId, dto.RoleId, performedBy);
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
