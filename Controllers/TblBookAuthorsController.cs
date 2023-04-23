using LibManagementAPI.Helper;
using LibManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblBookAuthorsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public TblBookAuthorsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/TblBookAuthors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblBookAuthorDTO>>> GetTblBookAuthors([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = null)
        {
            if (_context.TblBookAuthors == null)
            {
                return NotFound();
            }

            var totalItems = _context.TblBookAuthors.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var result = _context.TblBookAuthors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                result = result.Where(i => i.BookAuthorsAuthorName.Contains(keyword));
            }

            return await result.Select(x => ItemToDTO(x))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // GET: api/TblBookAuthors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblBookAuthorDTO>> GetTblBookAuthor(int id)
        {
            if (_context.TblBookAuthors == null)
            {
                return NotFound();
            }
            var tblBookAuthor = await _context.TblBookAuthors.Where(b => b.BookAuthorsAuthorId == id).SingleOrDefaultAsync();

            if (tblBookAuthor == null)
            {
                return NotFound();
            }

            return ItemToDTO(tblBookAuthor);
        }

        //PUT: api/TblBookAuthors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblBookAuthor(int id, TblBookAuthorDTO tblBookAuthorDTO)
        {
            if (id != tblBookAuthorDTO.BookAuthorsAuthorId)
            {
                return BadRequest();
            }

            var item = await _context.TblBookAuthors.FindAsync(id);

            item.BookAuthorsAuthorName = tblBookAuthorDTO.BookAuthorsAuthorName;
            item.BookAuthorsBookId = tblBookAuthorDTO.BookAuthorsBookId;

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblBookAuthorExists(id))
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

        // POST: api/TblBookAuthors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TblBookAuthorDTO>> PostTblBookAuthor(TblBookAuthorDTO tblBookAuthorDTO)
        {
            if (_context.TblBookAuthors == null)
            {
                return Problem("Entity set 'LibraryContext.TblBookAuthors'  is null.");
            }
            var item = new TblBookAuthor
            {
                BookAuthorsAuthorName = tblBookAuthorDTO.BookAuthorsAuthorName,
                BookAuthorsBookId = tblBookAuthorDTO.BookAuthorsBookId,
            };
            _context.TblBookAuthors.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTblBookAuthor), new { id = item.BookAuthorsAuthorId }, ItemToDTO(item));
        }

        // DELETE: api/TblBookAuthors/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblBookAuthor(int id)
        {
            if (_context.TblBookAuthors == null)
            {
                return NotFound();
            }
            var tblBookAuthor = await _context.TblBookAuthors.FindAsync(id);
            if (tblBookAuthor == null)
            {
                return NotFound();
            }

            _context.TblBookAuthors.Remove(tblBookAuthor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblBookAuthorExists(int id)
        {
            return (_context.TblBookAuthors?.Any(e => e.BookAuthorsAuthorId == id)).GetValueOrDefault();
        }

        private static TblBookAuthorDTO ItemToDTO(TblBookAuthor item) =>
            new TblBookAuthorDTO
            {
                BookAuthorsAuthorId = item.BookAuthorsAuthorId,
                BookAuthorsBookId = item.BookAuthorsBookId,
                BookAuthorsAuthorName = item.BookAuthorsAuthorName,
            };

        private async Task<bool> CheckRole()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];

            var userID = RetrieveInfoHelper.GetUserIdFromJWT(token);

            return await RoleHelper.IsAdminOfLibrary(_context, userID);

        }
    }
}
