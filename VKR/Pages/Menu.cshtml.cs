// Pages/Menu/Menu.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VKR.Models;

namespace YourNamespace.Pages.Menu
{
    public class MenuModel : PageModel
    {
        private readonly VkrContext _context;

        public MenuModel(VkrContext context)
        {
            _context = context;
        }

        public List<Dish> Dishes { get; set; }
        public List<Dishcategory> Categories { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SelectedCategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? MinCalories { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? MaxCalories { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Categories = await _context.Dishcategories.ToListAsync();

            var query = _context.Dishes.AsQueryable();

            if (SelectedCategoryId.HasValue)
            {
                query = query.Where(d => d.Dishcategoryid == SelectedCategoryId);
            }

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                query = query.Where(d => d.Dishname.Contains(SearchQuery));
            }

            if (MinCalories.HasValue)
            {
                query = query.Where(d => d.Calories >= MinCalories);
            }

            if (MaxCalories.HasValue)
            {
                query = query.Where(d => d.Calories <= MaxCalories);
            }

            Dishes = await query.ToListAsync();
            return Page();
        }
    }
}
