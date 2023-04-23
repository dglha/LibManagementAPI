using LibManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace LibManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblUsersController : ControllerBase
    {
        private readonly LibraryContext _context;
        public IConfiguration _configuration;

        public TblUsersController(LibraryContext context, IConfiguration config)
        {
            _context = context;
            _configuration = config;
        }

        // POST: api/TblUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Login")]
        public async Task<ActionResult<TblUser>> PostTblUserLogin(TblUserDTO tblUserDTO)
        {
            if (_context.TblUsers == null)
            {
                return Problem("Entity set 'LibraryContext.TblUsers'  is null.");
            }

            var tblUserLogin = await _context.TblUsers.Where(u => u.UserEmail == tblUserDTO.UserEmail).FirstAsync();
            if (tblUserLogin == null)
            {
                return BadRequest();
            }

            var verify = BCrypt.Net.BCrypt.Verify(tblUserDTO.UserPassword, tblUserLogin.UserPassword);

            if (!verify)
            {
                return BadRequest("Wrong email or password");
            }

            // Create JWT 
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", tblUserLogin.UserId.ToString()),
                        new Claim("DisplayName", tblUserLogin.UserDisplayName),
                        new Claim("Email", tblUserLogin.UserEmail),
                        new Claim("UserRole", tblUserLogin.UserRole),
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: signIn);
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
        private bool TblUserExists(int id)
        {
            return (_context.TblUsers?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
