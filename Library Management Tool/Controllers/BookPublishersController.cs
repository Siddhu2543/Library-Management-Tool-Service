using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library_Management_Tool.Models;

namespace Library_Management_Tool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookPublishersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookPublishersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/BookPublishers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookPublisher>>> GetBookPublishers()
        {
          if (_context.BookPublishers == null)
          {
              return NotFound();
          }
            return await _context.BookPublishers.ToListAsync();
        }

        // GET: api/BookPublishers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookPublisher>> GetBookPublisher(int id)
        {
          if (_context.BookPublishers == null)
          {
              return NotFound();
          }
            var bookPublisher = await _context.BookPublishers.FindAsync(id);

            if (bookPublisher == null)
            {
                return NotFound();
            }

            return bookPublisher;
        }

        // PUT: api/BookPublishers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookPublisher(int id, BookPublisher bookPublisher)
        {
            if (id != bookPublisher.Id)
            {
                return BadRequest();
            }

            _context.Entry(bookPublisher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookPublisherExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BookPublishers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookPublisher>> PostBookPublisher(BookPublisher bookPublisher)
        {
          if (_context.BookPublishers == null)
          {
              return Problem("Entity set 'AppDbContext.BookPublishers'  is null.");
          }
            _context.BookPublishers.Add(bookPublisher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookPublisher", new { id = bookPublisher.Id }, bookPublisher);
        }

        // DELETE: api/BookPublishers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookPublisher(int id)
        {
            if (_context.BookPublishers == null)
            {
                return NotFound();
            }
            var bookPublisher = await _context.BookPublishers.FindAsync(id);
            if (bookPublisher == null)
            {
                return NotFound();
            }

            _context.BookPublishers.Remove(bookPublisher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookPublisherExists(int id)
        {
            return (_context.BookPublishers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
