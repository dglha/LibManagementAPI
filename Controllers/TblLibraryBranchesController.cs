using LibManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblLibraryBranchesController : ControllerBase
    {
        private readonly LibraryContext _context;

        public TblLibraryBranchesController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/TblLibraryBranches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblLibraryBranchDTO>>> GetTblLibraryBranches()
        {
            if (_context.TblLibraryBranches == null)
            {
                return NotFound();
            }
            return await _context.TblLibraryBranches.Select(x => ItemToDTO(x)).ToListAsync();
        }

        // GET: api/TblLibraryBranches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblLibraryBranchDTO>> GetTblLibraryBranch(int id)
        {
            if (_context.TblLibraryBranches == null)
            {
                return NotFound();
            }
            var tblLibraryBranch = await _context.TblLibraryBranches.FindAsync(id);

            if (tblLibraryBranch == null)
            {
                return NotFound();
            }

            return ItemToDTO(tblLibraryBranch);
        }

        private bool TblLibraryBranchExists(int id)
        {
            return (_context.TblLibraryBranches?.Any(e => e.LibraryBranchBranchId == id)).GetValueOrDefault();
        }

        private static TblLibraryBranchDTO ItemToDTO(TblLibraryBranch item) => new TblLibraryBranchDTO
        {
            LibraryBranchBranchId = item.LibraryBranchBranchId,
            LibraryBranchBranchName = item.LibraryBranchBranchName,
            LibraryBranchBranchAddress = item.LibraryBranchBranchAddress,
        };
    }
}
