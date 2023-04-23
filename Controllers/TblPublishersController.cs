using LibManagementAPI.Helper;
using LibManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblPublishersController : ControllerBase
    {
        private readonly LibraryContext _context;

        public TblPublishersController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/TblPublishers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblPublisherDTO>>> GetTblPublishers()
        {
            if (_context.TblPublishers == null)
            {
                return NotFound();
            }
            return await _context.TblPublishers.Select(x => ItemToDTO(x)).ToListAsync();
        }

        // GET: api/TblPublishers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblPublisherDTO>> GetTblPublisher(string id)
        {
            if (_context.TblPublishers == null)
            {
                return NotFound();
            }
            var tblPublisher = await _context.TblPublishers.FindAsync(id);

            if (tblPublisher == null)
            {
                return NotFound();
            }

            return ItemToDTO(tblPublisher);
        }

        // PUT: api/TblPublishers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblPublisher(string id, TblPublisherDTO tblPublisherDTO)
        {
            if (id != tblPublisherDTO.PublisherPublisherName)
            {
                return BadRequest();
            }

            var tblPublisher = await _context.TblPublishers.FindAsync(id);
            if (tblPublisher == null)
            {
                return NotFound();
            }

            tblPublisher.PublisherPublisherName = tblPublisherDTO.PublisherPublisherName;
            tblPublisher.PublisherPublisherAddress = tblPublisherDTO.PublisherPublisherAddress;
            tblPublisher.PublisherPublisherPhone = tblPublisherDTO.PublisherPublisherPhone;

            _context.Entry(tblPublisher).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblPublisherExists(id))
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

        // POST: api/TblPublishers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TblPublisherDTO>> PostTblPublisher(TblPublisherDTO tblPublisherDTO)
        {
            if (_context.TblPublishers == null)
            {
                return Problem("Entity set 'LibraryContext.TblPublishers'  is null.");
            }

            var item = new TblPublisher {
                PublisherPublisherName = tblPublisherDTO.PublisherPublisherName,
                PublisherPublisherAddress = tblPublisherDTO.PublisherPublisherAddress,
                PublisherPublisherPhone = tblPublisherDTO.PublisherPublisherPhone
            };
            _context.TblPublishers.Add(item);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TblPublisherExists(item.PublisherPublisherName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetTblPublisher), new { id = item.PublisherPublisherName }, ItemToDTO(item));
        }

        // DELETE: api/TblPublishers/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTblPublisher(string id)
        //{
        //    if (_context.TblPublishers == null)
        //    {
        //        return NotFound();
        //    }
        //    var tblPublisher = await _context.TblPublishers.FindAsync(id);
        //    if (tblPublisher == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.TblPublishers.Remove(tblPublisher);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool TblPublisherExists(string id)
        {
            return (_context.TblPublishers?.Any(e => e.PublisherPublisherName == id)).GetValueOrDefault();
        }

        private static TblPublisherDTO ItemToDTO(TblPublisher publisher) =>
            new TblPublisherDTO
            {
                PublisherPublisherName = publisher.PublisherPublisherName,
                PublisherPublisherAddress = publisher.PublisherPublisherAddress,
                PublisherPublisherPhone = publisher.PublisherPublisherPhone
            };

        private async Task<bool> CheckRole()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];

            var userID = RetrieveInfoHelper.GetUserIdFromJWT(token);

            return await RoleHelper.IsAdminOfLibrary(_context, userID);

        }
    }
}
