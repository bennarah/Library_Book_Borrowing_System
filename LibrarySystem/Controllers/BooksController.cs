using LibrarySystem.DTOs;
using LibrarySystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;

        public BooksController(IBookService service)
        {
            _service = service;
        }

        // GET /api/books
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _service.GetAllAsync();
            return Ok(books);
        }

        // GET /api/books/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _service.GetByIdAsync(id);
            if (book == null) return NotFound(new { error = "Book not found." });
            return Ok(book);
        }

        // POST /api/books
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT /api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                if (updated == null) return NotFound(new { error = "Book not found." });
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE /api/books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { error = "Book not found." });
            return NoContent();
        }
    }
}
