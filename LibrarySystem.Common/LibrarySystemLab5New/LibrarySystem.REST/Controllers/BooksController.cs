using System.Threading.Tasks;                    // для Task<>
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibrarySystem.Infrastructure;               // для LibraryDbContext
using LibrarySystem.Infrastructure.Models;       // для Book

namespace LibrarySystem.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _context.Books.ToListAsync();
            return Ok(books);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        // POST: api/Books/5/reserve
        [HttpPost("{id}/reserve")]
        [Authorize(Roles = "User,Admin,Librarian")]
        public async Task<IActionResult> ReserveBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            // Здесь может быть любая логика резервирования
            return Ok(new { message = $"Книга ID={id} забронирована пользователем {User.Identity!.Name}" });
        }

        // POST: api/Books
        [HttpPost]
        [Authorize(Roles = "Librarian,Admin")]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Librarian,Admin")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book updated)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            book.Title = updated.Title;
            book.AuthorName = updated.AuthorName;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
