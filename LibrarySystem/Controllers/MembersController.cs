using LibrarySystem.DTOs;
using LibrarySystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    [ApiController]
    [Route("api/members")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _service;

        public MembersController(IMemberService service)
        {
            _service = service;
        }

        // GET /api/members
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var members = await _service.GetAllAsync();
            return Ok(members);
        }

        // GET /api/members/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var member = await _service.GetByIdAsync(id);
            if (member == null) return NotFound(new { error = "Member not found." });
            return Ok(member);
        }

        // POST /api/members
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MemberRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT /api/members/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MemberRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound(new { error = "Member not found." });
            return Ok(updated);
        }

        // DELETE /api/members/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { error = "Member not found." });
            return NoContent();
        }
    }
}
