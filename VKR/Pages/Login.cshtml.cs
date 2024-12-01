using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VKR.Models;

namespace VKR.Pages
{
    public class LoginModel : PageModel
    {
        private readonly VkrContext _context;
        private readonly IConfiguration _configuration;

        public LoginModel(VkrContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [BindProperty]
        public string Phonenumber { get; set; }
        [BindProperty]
        public string Passwordhash { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var user = _context.Users.FirstOrDefault(u => u.Phonenumber == Phonenumber);

            if (user == null || !BCrypt.Net.BCrypt.Verify(Passwordhash, user.Passwordhash))
            {
                ModelState.AddModelError("", "Неверный номер телефона или пароль.");
                return Page();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Fullname),
                    new Claim(ClaimTypes.MobilePhone, user.Phonenumber)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            ViewData["Token"] = tokenHandler.WriteToken(token);

            return RedirectToPage("Menu");
        }
    }
}
