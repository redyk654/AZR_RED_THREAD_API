using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AZR_RED_THREAD_BLL.Services.Access;
using AZR_RED_THREAD_BLL.DTOs.Access;
using System.Collections.Generic;


namespace AZR_RED_THREAD_API.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class PrivilegesController : ControllerBase
    {
        private readonly IPrivilegeServices _privServices;
        public PrivilegesController(IPrivilegeServices privServices)
        {
            _privServices = privServices;
        }
        // GET api/admin/privileges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrivilegeDto>>> GetAll() => Ok(await _privServices.GetAllPrivilegesAsync());

        // GET api/admin/privileges/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PrivilegeDto>> GetById(int id)
        {
            var p = await _privServices.GetPrivilegeByIdAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        // POST api/admin/privileges
        [HttpPost]
        public async Task<ActionResult<PrivilegeDto>> Create([FromBody] PrivilegeDto dto)
        {
            var userId = ResolveUserIdOrFallback();
            var created = await _privServices.CreatePrivilegeAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT api/admin/privileges/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<PrivilegeDto>> Update(int id, [FromBody] PrivilegeDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var userId = ResolveUserIdOrFallback();
            var updated = await _privServices.UpdatePrivilegeAsync(dto, userId);
            return Ok(updated);
        }

        // DELETE api/admin/privileges/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = ResolveUserIdOrFallback();
            var r = await _privServices.DeletePrivilegeAsync(id, userId);
            return r ? NoContent() : NotFound();
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