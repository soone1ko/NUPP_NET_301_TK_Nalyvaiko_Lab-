using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystem.Common;
using LibrarySystem.Infrastructure.Data;
using LibrarySystem.REST.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ICrudServiceAsync<Book> _bookService;

        public BooksController(ICrudServiceAsync<Book> bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
        {
            var books = await _bookService.ReadAllAsync();
            var result = new List<BookDto>();
            foreach (var b in books)
            {
                result.Add(new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Year = b.Year,
                    Description = b.Description
                });
            }
            return Ok(result);
        }

        // GET: api/Books/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BookDto>> GetById(Guid id)
        {
            var book = await _bookService.ReadAsync(id);
            if (book == null)
                return NotFound();

            var dto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Year = book.Year,
                Description = book.Description
            };
            return Ok(dto);
        }

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<BookDto>> Create([FromBody] BookDto dto)
        {
            var newBook = new Book
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Author = dto.Author,
                Year = dto.Year,
                Description = dto.Description
            };

            await _bookService.CreateAsync(newBook);

            var createdDto = new BookDto
            {
                Id = newBook.Id,
                Title = newBook.Title,
                Author = newBook.Author,
                Year = newBook.Year,
                Description = newBook.Description
            };

            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
        }

        // PUT: api/Books/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BookDto dto)
        {
            var existing = await _bookService.ReadAsync(id);
            if (existing == null)
                return NotFound();

            existing.Title = dto.Title;
            existing.Author = dto.Author;
            existing.Year = dto.Year;
            existing.Description = dto.Description;

            await _bookService.UpdateAsync(existing);

            return NoContent();
        }

        // DELETE: api/Books/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _bookService.ReadAsync(id);
            if (existing == null)
                return NotFound();

            await _bookService.RemoveAsync(existing);

            return NoContent();
        }
    }
}
