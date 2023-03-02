using LTMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LTMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase

    {
        public static CompteHash User = new CompteHash();


        
        public IConfiguration Configuration { get; }

        public AuthController(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        // POST: api/<AuthController>
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(Compte request)
        {
            CreatePasswordHash(request.Password, out byte[] PasswordHash, out byte[] PasswordSalt);

            User.Login = request.Login;
            User.PasswordHash = PasswordHash;
            User.PasswordSalt = PasswordSalt;


           



            return Ok(User);

        }
        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(Compte request)
        {
            if (User.Login != request.Login)
            {
                return BadRequest("User not found");
            }
            if (!verifyPasswordHash(request.Password, User.PasswordHash, User.PasswordSalt))
            {

                return BadRequest("wrong password");
            }
            string token = createToken(User);
            return Ok(token);
        }

        private string createToken(CompteHash user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Login)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                Configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        private bool verifyPasswordHash(string password, byte[] PasswordHash, byte[] PasswordSalt)

        {
            using (var hmac = new HMACSHA512(User.PasswordSalt))
            {

                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(PasswordHash);

            }
        }

    }
}
