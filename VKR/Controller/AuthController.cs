using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VKR.DTO;
using VKR.Models;

namespace VKR.Controllers
{
    [Route("api/auth")]
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

        // Регистрация
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            // Проверка на наличие данных
            if (user == null || string.IsNullOrWhiteSpace(user.Fullname) ||
                string.IsNullOrWhiteSpace(user.Passwordhash) ||
                string.IsNullOrWhiteSpace(user.Email))
            {
                return BadRequest("Все обязательные поля должны быть заполнены.");
            }

            // Проверяем, существует ли пользователь с таким номером телефона или email
            if (_context.Users.Any(u => u.Phonenumber == user.Phonenumber || u.Email == user.Email))
            {
                return Conflict("Пользователь с таким номером телефона или email уже существует.");
            }
            
            // Хешируем пароль
            user.Passwordhash = BCrypt.Net.BCrypt.HashPassword(user.Passwordhash);

            // Добавляем пользователя в базу данных
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("Пользователь успешно зарегистрирован.");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            // Проверка на наличие пользователя по номеру телефона
            var user = _context.Users.FirstOrDefault(u => u.Phonenumber == loginRequest.Phonenumber);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Passwordhash, user.Passwordhash))
            {
                return Unauthorized("Неверный номер телефона или пароль.");
            }

            // Генерация токена
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, user.Fullname),
            new Claim(ClaimTypes.MobilePhone, user.Phonenumber)
        }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"])),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Вернуть токен и URL для редиректа
            return Ok(new
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = token.ValidTo,
                RedirectUrl = Url.Page("/Menu") // Добавляем URL для редиректа
            });
        }





        [Authorize]
        [HttpGet("protected-resource")]
        public IActionResult GetProtectedData()
        {
            // Этот метод доступен только аутентифицированным пользователям
            return Ok(new { message = "Это защищённый ресурс!" });
        }

    }
}
