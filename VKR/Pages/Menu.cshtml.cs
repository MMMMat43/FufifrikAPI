using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VKR.Models;

namespace VKR.Pages;

public class MenuModel : PageModel
{
    private readonly VkrContext _context;

    public MenuModel(VkrContext context)
    {
        _context = context;
    }

    public List<Dishcategory> Categories { get; set; }

    public async Task OnGetAsync()
    {
        Categories = await _context.Dishcategories
            .Include(c => c.Dishes)
            .ToListAsync();
    }
}
