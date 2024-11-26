using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VKR.Models;

namespace VKR.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly VkrContext _context;

        public RegisterModel(VkrContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User InputUser { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            if (_context.Users.Any(u => u.Phonenumber == InputUser.Phonenumber || u.Email == InputUser.Email))
            {
                ModelState.AddModelError("", "Пользователь с таким номером телефона или email уже существует.");
                return Page();
            }

            InputUser.Passwordhash = BCrypt.Net.BCrypt.HashPassword(InputUser.Passwordhash);
            _context.Users.Add(InputUser);
            _context.SaveChanges();

            return RedirectToPage("Login");
        }
    }
}
