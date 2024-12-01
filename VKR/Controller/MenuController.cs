using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VKR.Models;

namespace VKR.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly VkrContext _context;

        public MenuController(VkrContext context)
        {
            _context = context;
        }

        [HttpGet("dishes")]
        public IActionResult GetDishes(int? categoryId, int? minCalories, int? maxCalories, string? searchTerm)
        {
            // Базовый запрос к блюдам
            var dishes = _context.Dishes.AsQueryable();

            // Фильтрация по категории
            if (categoryId.HasValue)
                dishes = dishes.Where(d => d.Dishcategoryid == categoryId.Value);

            // Фильтрация по калориям
            if (minCalories.HasValue)
                dishes = dishes.Where(d => d.Calories >= minCalories.Value);

            if (maxCalories.HasValue)
                dishes = dishes.Where(d => d.Calories <= maxCalories.Value);

            // Фильтрация по названию
            if (!string.IsNullOrEmpty(searchTerm))
                dishes = dishes.Where(d => d.Dishname.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            // Возврат результата
            return Ok(dishes.Select(d => new
            {
                d.Dishid,
                d.Dishname,
                d.Description,
                d.Price,
                d.Calories,
                d.Imageurl,
            }).ToList());
        }

        [HttpGet("dishes/{id}")]
        public IActionResult GetDishById(int id)
        {
            var dish = _context.Dishes
                .Where(d => d.Dishid == id)
                .Select(d => new
                {
                    d.Dishid,
                    d.Dishname,
                    d.Description,
                    d.Price,
                    d.Calories,
                    d.Imageurl
                })
                .FirstOrDefault();

            if (dish == null)
                return NotFound("Блюдо не найдено.");

            return Ok(dish);
        }

        [HttpPost("cart/add")]
        public IActionResult AddToCart(int dishId, int quantity)
        {
            // Проверка наличия ингредиентов
            var ingredients = _context.Ingredientamounts
                .Where(ia => ia.Dishid == dishId)
                .ToList();

            foreach (var ingredient in ingredients)
            {
                var stock = _context.Ingredients.FirstOrDefault(i => i.Ingredientid == ingredient.Ingredientid);
                if (stock == null || stock.Quantity < ingredient.Amount * quantity)
                    return BadRequest($"Недостаточно ингредиентов для блюда: {ingredient.Ingredientid}");

                stock.Quantity -= ingredient.Amount * quantity;
            }

            // Сохранение изменений
            _context.SaveChanges();

            // Обновление корзины
            // Логика добавления в корзину опущена. Она может включать запись в таблицу OrderItems.

            return Ok("Блюдо добавлено в корзину.");
        }

        [HttpPost("cart/remove")]
        public IActionResult RemoveFromCart(int dishId, int quantity)
        {
            // Возврат ингредиентов
            var ingredients = _context.Ingredientamounts
                .Where(ia => ia.Dishid == dishId)
                .ToList();

            foreach (var ingredient in ingredients)
            {
                var stock = _context.Ingredients.FirstOrDefault(i => i.Ingredientid == ingredient.Ingredientid);
                if (stock != null)
                    stock.Quantity += ingredient.Amount * quantity;
            }

            // Сохранение изменений
            _context.SaveChanges();

            // Логика удаления из корзины опущена.

            return Ok("Блюдо удалено из корзины.");
        }
    }
}
