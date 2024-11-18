using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VKR.Models;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly VkrContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(VkrContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Регистрация пользователя
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            // Проверка: существует ли пользователь с таким логином или email
            if (await _context.Users.AnyAsync(u => u.Fullname == user.Fullname || u.Email == user.Email))
            {
                return BadRequest(new { message = "Пользователь с таким логином или email уже существует." });
            }

            // Хэшируем пароль
            user.Passwordhash = BCrypt.Net.BCrypt.HashPassword(user.Passwordhash);

            // Сохраняем пользователя
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Пользователь успешно зарегистрирован." });
        }

        // Авторизация пользователя
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginData)
        {
            // Ищем пользователя по логину
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginData.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginData.Password, user.Password))
            {
                return Unauthorized(new { message = "Неверный логин или пароль." });
            }

            // Генерируем JWT токен
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("UserId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtIssuer"],
                audience: _configuration["JwtAudience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = DateTime.Now.AddHours(1)
            });
        }
    }
}
