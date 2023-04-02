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
            return await _context.Issues.Where(i => i.Status != "Returned").ToListAsync();
        }

        [HttpGet("Members/{id}")]
        public async Task<ActionResult<IEnumerable<Issue>>> GetMemberIssues(int id)
        {
            if (_context.Issues == null)
            {
                return NotFound();
            }
            return await _context.Issues.Where((i) => i.MemberId == id && i.Status == "Issued" || i.Status == "Renewed").ToListAsync();
        }

        [HttpGet("Renew/{id}")]
        public async Task<ActionResult<Issue>> RenewBook(int id)
        {
            if (_context.Issues == null)
            {
                return NotFound();
            }
            var issue = await _context.Issues.FindAsync(id);
            if(issue == null)
            {
                return NotFound();
            }
            issue.DueDate = issue.DueDate.AddMonths(1);
            issue.Status = "Renewed";
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

            return CreatedAtAction("GetIssue", new { id = issue.Id }, issue);
        }

        [HttpGet("Return/{id}")]
        public async Task<ActionResult> ReturnBook(int id)
        {
            if (_context.Issues == null)
            {
                return NotFound();
            }
            var issue = await _context.Issues.FindAsync(id);
            var book = await _context.Books.FindAsync(issue.BookId);

            if (issue == null || book == null)
            {
                return NotFound();
            }

            issue.ReturnDate = DateTime.Now;
            issue.Status = "Returned";
            book.Availability += 1;

            _context.Entry(issue).State = EntityState.Modified;
            _context.Entry(book).State = EntityState.Modified;

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

            return CreatedAtAction("GetIssue", new { id = issue.Id }, issue);
        }

        [HttpGet("Books/{id}")]
        public async Task<ActionResult<IEnumerable<Issue>>> GetBookIssues(int id)
        {
            if (_context.Issues == null)
            {
                return NotFound();
            }
            return await _context.Issues.Where((i) => i.BookId == id && i.Status == "Issued" || i.Status == "Renewed").ToListAsync();
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
            var count = _context.Issues.Where(i => i.MemberId == userId && i.Status == "Issued" || i.Status == "Renewed").Count();
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
            var count = _context.Issues.Where(i => i.BookId== bookId && i.Status == "Issued" || i.Status == "Renewed").Count();
            return Ok(count);
        }

        [HttpGet("IsBookIssuedToMember/{bookId}/{memberId}")]
        public async Task<ActionResult<bool>> IsBookIssuedToMember(int bookId, int memberId)
        {
            bool doesBookExist = (_context.Books?.Any(b => b.Id == bookId)).GetValueOrDefault();
            bool doesMemberExist = (_context.Members?.Any(m => m.Id == memberId)).GetValueOrDefault();
            if(doesBookExist && doesMemberExist)
            {
                var result = _context.Issues.Where(i => i.BookId == bookId && i.MemberId == memberId && (i.Status == "Issued" || i.Status == "Renewed")).Any();

                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("Search/Book/{name}")]
        public async Task<ActionResult<IEnumerable<Issue>>> SearchIssueByBookName(string name)
        {
            if (_context.Issues == null)
            {
                return NotFound();
            }

            var issues = _context.Issues.ToList().Where((i) => i.Status != "Returned" && _context.Books.Find(i.BookId).Title.ToUpper().Contains(name.ToUpper())).ToList();

            if (issues.Count == 0)
            {
                return NotFound();
            }

            return issues;
        }

        [HttpGet("Search/Member/{name}")]
        public async Task<ActionResult<IEnumerable<Issue>>> SearchIssueByMemberName(string name)
        {
            if (_context.Issues == null)
            {
                return NotFound();
            }
            var issues = _context.Issues.ToList().Where((i) => i.Status != "Returned" && _context.Members.Find(i.MemberId).Name.ToUpper().Contains(name.ToUpper())).ToList();

            if (issues.Count == 0)
            {
                return NotFound();
            }

            return issues;
        }

        private bool IssueExists(int id)
        {
            return (_context.Issues?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
