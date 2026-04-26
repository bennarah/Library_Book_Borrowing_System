using LibrarySystem.DTOs;
using LibrarySystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    [ApiController]
    [Route("api/borrows")]
    public class BorrowRecordsController : ControllerBase
    {
        private readonly IBorrowService _service;

        public BorrowRecordsController(IBorrowService service)
        {
            _service = service;
        }

        // GET /api/borrows
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _service.GetAllAsync();
            return Ok(records);
        }

        // GET /api/borrows/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _service.GetByIdAsync(id);
            if (record == null) return NotFound(new { error = "Borrow record not found." });
            return Ok(record);
        }

        // POST /api/borrows
        [HttpPost]
        public async Task<IActionResult> Borrow([FromBody] BorrowRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var created = await _service.BorrowAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT /api/borrows/{id}/return
        [HttpPut("{id}/return")]
        public async Task<IActionResult> Return(int id)
        {
            try
            {
                var result = await _service.ReturnAsync(id);
                if (result == null) return NotFound(new { error = "Borrow record not found." });
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
