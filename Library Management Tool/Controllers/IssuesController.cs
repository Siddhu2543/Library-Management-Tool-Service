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
    public class IssuesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IssuesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Issues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Issue>>> GetIssues()
        {
          if (_context.Issues == null)
          {
              return NotFound();
          }
            return await _context.Issues.ToListAsync();
        }

        // GET: api/Issues/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Issue>> GetIssue(int id)
        {
          if (_context.Issues == null)
          {
              return NotFound();
          }
            var issue = await _context.Issues.FindAsync(id);

            if (issue == null)
            {
                return NotFound();
            }

            return issue;
        }

        // PUT: api/Issues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIssue(int id, Issue issue)
        {
            if (id != issue.Id)
            {
                return BadRequest();
            }

            _context.Entry(issue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IssueExists(id))
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

        // POST: api/Issues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Issue>> PostIssue(Issue issue)
        {
          if (_context.Issues == null)
          {
              return Problem("Entity set 'AppDbContext.Issues'  is null.");
          }
            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIssue", new { id = issue.Id }, issue);
        }

        // DELETE: api/Issues/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIssue(int id)
        {
            if (_context.Issues == null)
            {
                return NotFound();
            }
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null)
            {
                return NotFound();
            }

            _context.Issues.Remove(issue);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("CurrentMemberIssueCount/{userId}")]
        public async Task<ActionResult<int>> GetCurrentMemberIssueCount(int userId)
        {
            bool doesMemberExist = (_context.Members?.Any(m => m.Id == userId)).GetValueOrDefault();
            if (!doesMemberExist)
            {
                return BadRequest();
            }
            var count = _context.Issues.Where(i => i.MemberId == userId && i.Status == "Issued").Count();
            return Ok(count);
        }

        [HttpGet("CurrentBookIssueCount/{bookId}")]
        public async Task<ActionResult<int>> GetCurrentBookIssueCount(int bookId)
        {
            bool doesBookExist = (_context.Books?.Any(b => b.Id == bookId)).GetValueOrDefault();
            if (!doesBookExist)
            {
                return BadRequest();
            }
            var count = _context.Issues.Where(i => i.BookId== bookId && i.Status == "Issued").Count();
            return Ok(count);
        }

        [HttpGet("IsBookIssuedToMember/{bookId}/{memberId}")]
        public async Task<ActionResult<bool>> IsBookIssuedToMember(int bookId, int memberId)
        {
            bool doesBookExist = (_context.Books?.Any(b => b.Id == bookId)).GetValueOrDefault();
            bool doesMemberExist = (_context.Members?.Any(m => m.Id == memberId)).GetValueOrDefault();
            if(doesBookExist && doesMemberExist)
            {
                var result = _context.Issues.Where(i => i.BookId == bookId && i.MemberId == memberId && i.Status == "Issued").Any();

                return Ok(result);
            }
            return BadRequest();
        }

        private bool IssueExists(int id)
        {
            return (_context.Issues?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
