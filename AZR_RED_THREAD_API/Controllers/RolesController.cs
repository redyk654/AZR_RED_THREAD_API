using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AZR_RED_THREAD_BLL.Services.Access;
using AZR_RED_THREAD_BLL.DTOs.Access;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;


namespace AZR_RED_THREAD_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleServices _roleServices;
        public RolesController(IRoleServices roleServices)
        {
            _roleServices = roleServices;
        }

        // GET api/roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAll()
        {
            return Ok(await _roleServices.GetAllRolesAsync());
        }

        // GET api/roles/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetById(int id)
        {
            var r = await _roleServices.GetRoleByIdAsync(id);
            if (r == null) return NotFound();
            return Ok(r);
        }

        // POST api/roles
        [HttpPost]
        public async Task<ActionResult<RoleDto>> Create([FromBody] CreateRoleDto dto)
        {
            // in production: check user authorization (only admins)
            var userId = ResolveUserIdOrFallback();
            var created = await _roleServices.CreateRoleAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT api/roles/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<RoleDto>> Update(int id, [FromBody] UpdateRoleDto dto)
        {
            if (id != dto.Id) return BadRequest("Id mismatch");
            var userId = ResolveUserIdOrFallback();
            var updated = await _roleServices.UpdateRoleAsync(dto, userId);
            return Ok(updated);
        }

        // DELETE api/roles/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = ResolveUserIdOrFallback();
            var r = await _roleServices.DeleteRoleAsync(id, userId);
            return r ? NoContent() : NotFound();
        }

        // GET api/roles/{id}/privileges
        [HttpGet("{id}/privileges")]
        public async Task<ActionResult<IEnumerable<PrivilegeDto>>> GetRolePrivileges(int id)
        {
            return Ok(await _roleServices.GetPrivilegesForRoleAsync(id));
        }

        // POST api/roles/{id}/privileges
        [HttpPost("{id}/privileges")]
        public async Task<ActionResult> AddPrivilegeToRole(int id, [FromBody] int privilegeId)
        {
            var userId = ResolveUserIdOrFallback();
            await _roleServices.AddPrivilegeToRoleAsync(id, privilegeId, userId);
            return NoContent();
        }

        // DELETE api/roles/{id}/privileges/{privilegeId}
        [HttpDelete("{id}/privileges/{privilegeId}")]
        public async Task<ActionResult> RemovePrivilege(int id, int privilegeId)
        {
            var userId = ResolveUserIdOrFallback();
            await _roleServices.RemovePrivilegeFromRoleAsync(id, privilegeId, userId);
            return NoContent();
        }

        // Helper: resolve user id either from claim oid -> DB or fallback X-Fake-UserId (dev)
        private int ResolveUserIdOrFallback()
        {
            // if claims exist use them - simplified: try claim nameidentifier as int else fallback
            var claimVal = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(claimVal, out int id)) return id;

            // fallback header for dev
            if (Request.Headers.TryGetValue("X-Fake-UserId", out var h) && int.TryParse(h, out int hid)) return hid;

            return 1; // system by default (not ideal: in production force 401)
        }
    }
}