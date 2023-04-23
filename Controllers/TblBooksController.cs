using LibManagementAPI.Helper;
using LibManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblBooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public TblBooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/TblBooks
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<TblBookDTO>>> GetTblBooks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = null)
        //{
        //    if (_context.TblBooks == null)
        //    {
        //        return NotFound();
        //    }

        //    var totalItems = _context.TblBooks.Count();
        //    var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        //    var result = _context.TblBooks.AsQueryable();

        //    if (!string.IsNullOrWhiteSpace(keyword))
        //    {
        //        result = result.Where(i => i.BookTitle.Contains(keyword));
        //    }

        //    return await result.Select(x => ItemToDTO(x))
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();
        //}

        [HttpGet]
        public async Task<ActionResult> GetTblBooks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = null)
        {
            if (_context.TblBooks == null)
            {
                return NotFound();
            }

            var totalItems = _context.TblBooks.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var result = _context.TblBooks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                result = result.Where(i => i.BookTitle.Contains(keyword));
            }

            var items = await result.Include(b => b.TblBookAuthors)
                .Include(b => b.TblBookCopies)
                    .ThenInclude(cb => cb.BookCopiesBranch)
                .Select(x => ItemToFullDTO(x))
                //.Skip((pageNumber - 1) * pageSize)
                //.Take(pageSize)
                .ToListAsync();
            var response = new
            {
                TotalPages = totalPages,
                PageNumber = pageNumber,
                Items = items
            };

            return Ok(response);
        }

        // GET: api/TblBooks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblBookDTO>> GetTblBook(int id)
        {
            if (_context.TblBooks == null)
            {
                return NotFound();
            }
            var tblBook = await _context.TblBooks
                .Include(b => b.TblBookAuthors)
                .Include(b => b.TblBookCopies)
                    .ThenInclude(cb => cb.BookCopiesBranch)
                .Where(b => b.BookBookId == id)
                .FirstOrDefaultAsync();

            if (tblBook == null)
            {
                return NotFound();
            }

            return ItemToFullDTO(tblBook);
        }

        // PUT: api/TblBooks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblBook(int id, TblBookDTO tblBookDTO)
        {
            if (id != tblBookDTO.BookBookId)
            {
                return BadRequest();
            }

            var check = await CheckRole();

            if (!check)
            {
                return BadRequest("User has no access to this Library!");
            }

            var item = await _context.TblBooks.FindAsync(id);

            item.BookTitle = tblBookDTO.BookTitle;
            item.BookPublisherName = tblBookDTO.BookPublisherName;

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblBookExists(id))
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

        // POST: api/TblBooks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TblBookDTO>> PostTblBook(TblBookDTO tblBookDTO)
        {
            if (_context.TblBooks == null)
            {
                return Problem("Entity set 'LibraryContext.TblBooks'  is null.");
            }
            var check = await CheckRole();

            if (!check)
            {
                return BadRequest("User has no access to this Library!");
            }

            var item = new TblBook
            {
                BookPublisherName = tblBookDTO.BookPublisherName,
                BookTitle = tblBookDTO.BookTitle,
            };
            _context.TblBooks.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTblBook), new { id = item.BookBookId }, item);
        }

        // DELETE: api/TblBooks/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblBook(int id)
        {
            if (_context.TblBooks == null)
            {
                return NotFound();
            }

            var check = await CheckRole();

            if (!check)
            {
                return BadRequest("User has no access to this Library!");
            }

            var tblBook = await _context.TblBooks.FindAsync(id);

            if (tblBook == null)
            {
                return NotFound();
            }

            var tblBookCopy = await _context.TblBookCopies.Where(x => x.BookCopiesBookId == tblBook.BookBookId).FirstOrDefaultAsync();
            if (tblBookCopy == null)
            {
                return NotFound("Book not found in current lib.");
            }

            _context.TblBookCopies.Remove(tblBookCopy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Add copy to book
        [Authorize]
        [HttpPost("{id}/AddCopy")]
        public async Task<IActionResult> PostTblBookAddCopy(int id, TblBookCopyDTO bookCopyDTO)
        {
            if (!TblBookExists(id))
            {
                return NotFound();
            }

            var check = await CheckRole();

            if (!check)
            {
                return BadRequest("User has no access to this Library!");
            }

            var userRole = GetRole();

            if (bookCopyDTO.BookCopiesBranchId != userRole && userRole != 3)
            {
                return BadRequest("User can not perform action to another Library!");
            }

            var checkCopy = await _context.TblBookCopies.Where(x => x.BookCopiesBookId == id && x.BookCopiesBranchId == bookCopyDTO.BookCopiesBranchId).FirstOrDefaultAsync();
            if (checkCopy == null)
            {
                var item = new TblBookCopy
                {
                    BookCopiesBookId = id,
                    BookCopiesBranchId = bookCopyDTO.BookCopiesBranchId,
                    BookCopiesNoOfCopies = bookCopyDTO.BookCopiesNoOfCopies,
                };

                _context.TblBookCopies.Add(item);
                await _context.SaveChangesAsync();
                return Ok(item);
            }

            checkCopy.BookCopiesNoOfCopies += bookCopyDTO.BookCopiesNoOfCopies;
            _context.Entry(checkCopy).State = EntityState.Modified;
            //var item = new TblBookCopy
            //{
            //    BookCopiesBookId = id,
            //    BookCopiesBranchId = bookCopyDTO.BookCopiesBranchId,
            //    BookCopiesNoOfCopies = bookCopyDTO.BookCopiesNoOfCopies,
            //};

            //_context.TblBookCopies.Add(item);

            await _context.SaveChangesAsync();

            return Ok(checkCopy);


        }

        // Book's Authors


        private bool TblBookExists(int id)
        {
            return (_context.TblBooks?.Any(e => e.BookBookId == id)).GetValueOrDefault();
        }

        private static TblBookDTO ItemToFullDTO(TblBook book) => new TblBookDTO
        {
            BookBookId = book.BookBookId,
            BookTitle = book.BookTitle,
            BookPublisherName = book.BookPublisherName,
            TblBookAuthors = (from author in book.TblBookAuthors
                              select new TblBookAuthorDTO
                              {
                                  BookAuthorsAuthorName = author.BookAuthorsAuthorName,
                                  BookAuthorsAuthorId = author.BookAuthorsAuthorId,
                                  BookAuthorsBookId = author.BookAuthorsBookId,
                              }
                              ).ToList(),
            TblBookCopies = (from copy in book.TblBookCopies
                             select new TblBookCopyDTO
                             {
                                 BookCopiesBranch = new TblLibraryBranchDTO { LibraryBranchBranchName = copy.BookCopiesBranch.LibraryBranchBranchName, LibraryBranchBranchId = copy.BookCopiesBranch.LibraryBranchBranchId},
                                 BookCopiesBranchId = copy.BookCopiesBranchId,
                                 BookCopiesNoOfCopies = copy.BookCopiesNoOfCopies,
                                 BookCopiesCopiesId = copy.BookCopiesCopiesId,
                             }
                             ).ToList(),
        };

        private static TblBookDTO ItemToDTO(TblBook book) => new TblBookDTO
        {
            BookBookId = book.BookBookId,
            BookTitle = book.BookTitle,
            BookPublisherName = book.BookPublisherName,
        };

        private async Task<bool> CheckRole()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];

            var userID = RetrieveInfoHelper.GetUserIdFromJWT(token);

            return await RoleHelper.IsAdminOfLibrary(_context, userID);
            
        }     

        private int? GetRole()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];

            var userRole = int.Parse(RetrieveInfoHelper.GetUserRoleFromJWT(token));

            return userRole;
        }
    }
}
