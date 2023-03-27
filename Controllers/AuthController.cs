using LTMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;

using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace LTMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LtmsContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthController(LtmsContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        // POST: api/<AuthController>
        // POST: api/<AuthController>
        [HttpPost("register")]
        public async Task<ActionResult<Compte>> Register(Compte request)
        {
            var existingUser = await _dbContext.Comptes.FirstOrDefaultAsync(c => c.Login == request.Login);

            if (existingUser != null)
            {
                return BadRequest("User already exists");
            }

            await _dbContext.Comptes.AddAsync(request);
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var compte = new CompteHash
            {
                Login = request.Login,
                PasswordHash = passwordHash.ToArray(),
                PasswordSalt = passwordSalt.ToArray()
            };

            await _dbContext.CompteHashes.AddAsync(compte);
            await _dbContext.SaveChangesAsync();

            return Ok(compte);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Compte>> Login(Compte request)
        {
            var login = new Compte { Login = request.Login };
            var compte = await _dbContext.CompteHashes.FirstOrDefaultAsync(c => c.Login == login.Login);

            if (compte == null)
            {
                return BadRequest("User not found");
            }

            if (!VerifyPasswordHash(request.Password, compte.PasswordHash, compte.PasswordSalt))
            {
                return BadRequest("Wrong Password");
            }


            var token = CreateToken(compte);
            return Ok(new { message = "Login successful",token });
        }





        private string CreateToken(CompteHash compte)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, compte.Login),
                new Claim(ClaimTypes.NameIdentifier, compte.Id.ToString())
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

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            var user = await _dbContext.Comptes.FirstOrDefaultAsync(c => c.Login == email);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var newPassword = GenerateRandomPassword();
            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            var compteHash = await _dbContext.CompteHashes.FirstOrDefaultAsync(ch => ch.Login == user.Login);
            if (compteHash == null)
            {
                compteHash = new CompteHash()
                {
                    Login = user.Login,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };
                await _dbContext.CompteHashes.AddAsync(compteHash);
            }
            else
            {
                compteHash.PasswordHash = passwordHash;
                compteHash.PasswordSalt = passwordSalt;
            }

            await _dbContext.SaveChangesAsync();

           SendEmail(user.Login, newPassword);

            return Ok(new { message = "Password reset email sent" });
        }







        private string GenerateRandomPassword(int length = 8)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            var random = new Random();

            return new string(Enumerable.Repeat(validChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }







        [HttpPost]
        public void SendEmail(string mail, string newPassword)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("mayssaneji6@gmail.com"));
            email.To.Add(MailboxAddress.Parse(mail));
            email.Subject = ("Password Reset");
            email.Body = new TextPart(TextFormat.Html) { Text = "Your new password is: " + newPassword };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("mayssaneji6@gmail.com", "sousou msk");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}