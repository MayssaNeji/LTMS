using LTMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace LTMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LtmsContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthController(LtmsContext  dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        // POST: api/<AuthController>
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(Compte request)
        {    await _dbContext.Comptes.AddAsync(request);
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var compte = new CompteHash
            {
                Login = request.Login,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _dbContext.CompteHashes.AddAsync(compte);
            await _dbContext.SaveChangesAsync();

            return Ok(compte);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(Compte request)
        {
            var login = new Compte { Login = request.Login };
            var compte = await _dbContext.CompteHashes.FirstOrDefaultAsync(c => c.Login == login.Login);

            if (compte == null)
            {
                return BadRequest("User not found");
            }

            if (VerifyPasswordHash(request.Password, compte.PasswordHash, compte.PasswordSalt))
            {
                return BadRequest("Wrong password");
            }

            var token = CreateToken(compte);
            return Ok(token);
        }

        private string CreateToken(CompteHash compte)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, compte.Login)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }



        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    }
}
