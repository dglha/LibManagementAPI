using LibManagementAPI.Helper;
using LibManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblBorrowersController : ControllerBase
    {
        private readonly LibraryContext _context;

        public TblBorrowersController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/TblBorrowers
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<TblBorrowerDTO>>> GetTblBorrowers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = null)
        //{
        //    if (_context.TblBorrowers == null)
        //    {
        //        return NotFound();
        //    }

        //    var totalItems = _context.TblBorrowers.Count();
        //    var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        //    var result = _context.TblBorrowers.AsQueryable();

        //    if (!string.IsNullOrWhiteSpace(keyword))
        //    {
        //        result = result.Where(i => i.BorrowerBorrowerName.Contains(keyword));
        //    }

        //    return await result.Select(x => ItemToDTO(x))
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();
        //}

        [HttpGet]
        public async Task<ActionResult> GetTblBorrowers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = null)
        {
            if (_context.TblBorrowers == null)
            {
                return NotFound();
            }

            var totalItems = _context.TblBorrowers.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var result = _context.TblBorrowers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                result = result.Where(i => i.BorrowerBorrowerName.Contains(keyword));
            }

            var items = await result.Select(x => ItemToDTO(x))
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

        // GET: api/TblBorrowers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblBorrowerDTO>> GetTblBorrower(int id)
        {
            if (_context.TblBorrowers == null)
            {
                return NotFound();
            }
            var tblBorrower = await _context.TblBorrowers.FindAsync(id);

            if (tblBorrower == null)
            {
                return NotFound();
            }

            return ItemToDTO(tblBorrower);
        }

        // PUT: api/TblBorrowers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblBorrower(int id, TblBorrowerDTO tblBorrowerDTO)
        {
            var tblBorrower = await _context.TblBorrowers.FindAsync(id);

            if (tblBorrower == null)
            {
                return NotFound();
            }

            if (id != tblBorrower.BorrowerCardNo)
            {
                return BadRequest();
            }

            tblBorrower.BorrowerBorrowerName = tblBorrowerDTO.BorrowerBorrowerName;
            tblBorrower.BorrowerBorrowerAddress = tblBorrowerDTO.BorrowerBorrowerAddress;

            _context.Entry(tblBorrower).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblBorrowerExists(id))
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

        // POST: api/TblBorrowers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TblBorrowerDTO>> PostTblBorrower(TblBorrowerDTO tblBorrowerDTO)
        {
            if (_context.TblBorrowers == null)
            {
                return Problem("Entity set 'LibraryContext.TblBorrowers'  is null.");
            }

            var borrower = new TblBorrower
            {
                BorrowerBorrowerName = tblBorrowerDTO.BorrowerBorrowerName,
                BorrowerBorrowerAddress = tblBorrowerDTO.BorrowerBorrowerAddress,
                BorrowerBorrowerPhone = tblBorrowerDTO.BorrowerBorrowerPhone,
            };

            _context.TblBorrowers.Add(borrower);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTblBorrower), new { id = borrower.BorrowerCardNo }, ItemToDTO(borrower));
        }

        // DELETE: api/TblBorrowers/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblBorrower(int id)
        {
            if (_context.TblBorrowers == null)
            {
                return NotFound();
            }
            var tblBorrower = await _context.TblBorrowers.FindAsync(id);
            if (tblBorrower == null)
            {
                return NotFound();
            }

            _context.TblBorrowers.Remove(tblBorrower);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Custom API

        // POST : api/TblBorrower/5/Loan
        [Authorize]
        [HttpPost("{id}/Loan")]
        public async Task<ActionResult<TblBookLoan>> PostTblBorrowerNewLoan(int id, TblBookLoanDTO loan)
        {
            if (!TblBorrowerExists(id))
            {
                return BadRequest();
            }

            var check = await CheckRole();

            if (!check)
            {
                return BadRequest("User has no access to this Library!");
            }

            var userRole = GetRole();

            if (loan.BookLoansBranchId != userRole && userRole != 3){
                return BadRequest("User can not perform action to another Library!");
            }

            var bookCopies = await _context.TblBookCopies.Where(x => x.BookCopiesBookId == loan.BookLoansBookId).FirstOrDefaultAsync();
            if (bookCopies == null || bookCopies.BookCopiesNoOfCopies <= 0)
            {
                return BadRequest("Book does not exists or out of stock!");
            }

            var bookLoan = new TblBookLoan
            {
                BookLoansCardNo = id,
                BookLoansBookId = loan.BookLoansBookId,
                BookLoansBranchId = loan.BookLoansBranchId,
                BookLoansDateOut = loan.BookLoansDateOut,
                BookLoansDueDate = loan.BookLoansDueDate,
            };

            _context.TblBookLoans.Add(bookLoan);

            bookCopies.BookCopiesNoOfCopies--;
            _context.Entry(bookCopies).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return bookLoan;

        }

        // POST : api/TblBorrower/5/Return
        [Authorize]
        [HttpPost("{id}/Return/{loanId}")]
        public async Task<ActionResult<TblBookLoan>> PostTblBorrowerReturnLoan(int id, int loanId,TblBookLoanDTO loan)
        {
            if (!TblBorrowerExists(id))
            {
                return BadRequest();
            }

            var bookCopies = await _context.TblBookCopies.Where(x => x.BookCopiesBookId == loan.BookLoansBookId).FirstOrDefaultAsync();
            if (bookCopies == null)
            {
                return BadRequest("Book does not exists!");
            }

            bookCopies.BookCopiesNoOfCopies++;
            _context.Entry(bookCopies).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok();

        }

        // GET : api/TblBorrower/5/loan
        [HttpGet("{id}/loan")]
        public async Task<ActionResult<IEnumerable<TblBookLoanDTO>>> GetTblBorrowerLoans(int id)
        {
            if (!TblBorrowerExists(id))
            {
                return BadRequest();
            }

            var loans = await _context.TblBookLoans.Where(x => x.BookLoansCardNo == id)
                .OrderByDescending(x => x.BookLoansDateOut)
                .Include(x => x.BookLoansBook)
                .Include(x => x.BookLoansBranch)
                .Select(x => LoanItemToDTO(x))
                .ToListAsync();

            return loans;

        }

        private bool TblBorrowerExists(int id)
        {
            return (_context.TblBorrowers?.Any(e => e.BorrowerCardNo == id)).GetValueOrDefault();
        }

        private static TblBorrowerDTO ItemToDTO(TblBorrower item) => new TblBorrowerDTO
        {
            BorrowerCardNo = item.BorrowerCardNo,
            BorrowerBorrowerAddress = item.BorrowerBorrowerAddress,
            BorrowerBorrowerName = item.BorrowerBorrowerName,
            BorrowerBorrowerPhone = item.BorrowerBorrowerPhone,
        };

        private static TblBookLoanDTO LoanItemToDTO(TblBookLoan item) => new TblBookLoanDTO
        {
            BookLoansLoansId = item.BookLoansLoansId,
            BookLoansCardNo = item.BookLoansCardNo,
            BookLoansBranchId = item.BookLoansBranchId,
            BookLoansBookId = item.BookLoansBookId,
            BookLoansBranch = item.BookLoansBranch,
            BookLoansBook = new TblBookDTO
            {
                BookBookId = item.BookLoansBook.BookBookId,
                BookTitle = item.BookLoansBook.BookTitle,
                BookPublisherName = item.BookLoansBook.BookPublisherName
            },
            BookLoansDateOut = item.BookLoansDateOut,
            BookLoansDueDate = item.BookLoansDueDate,
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
