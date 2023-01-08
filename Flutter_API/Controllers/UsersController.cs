using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Flutter_API.Data;
using Flutter_API.Model;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;

namespace Flutter_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Flutter_APIContext _context;

        public UsersController(Flutter_APIContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{email}")]
        public async Task<ActionResult<User>> GetUser(String email)
        {
            var user = _context.User.Where(u => u.Email == email).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            user.Id = Guid.NewGuid();
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginRequest request)
        {
            var foundUser = _context.User.Where(u => u.Email == request.Email && u.Password == request.Password).FirstOrDefault();

            if (foundUser == null)
            {
                return BadRequest(new { message = "Invalid email or password" });
            }
            else if (foundUser.Email != request.Email)
            {
                return BadRequest(new { message = "Email not found" });
            }

            // Generate a JWT token
            var token = GenerateToken(foundUser);

            // Include the token in the response body
            return Ok(new { user = foundUser, token = token });
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Remove the user's authentication from the request
            HttpContext.SignOutAsync();

            // Return a response indicating that the user has been logged out
            return Ok("You have been logged out");
        }


        private static string GenerateToken(User user)
        {
            // Generate a random key with 256 bits
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("n").Substring(0, 32)));

            // Define the claims for the token
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            // Define the signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: "your-issuer",
                audience: "your-audience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            // Return the JWT token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        // DELETE: api/Users/5
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser(String email)
        {
            var user = _context.User.Where(u => u.Email == email).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string email)
        {
            return _context.User.Any(e => e.Email == email );
        }
    }

}
