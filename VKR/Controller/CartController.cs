using Microsoft.AspNetCore.Mvc;
using VKR.ICartService;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("count")]
    public IActionResult GetCartCount()
    {
        var count = _cartService.GetCartCount();
        return Ok(new { count });
    }

    [HttpPost("add")]
    public IActionResult AddToCart(int dishId, int quantity)
    {
        _cartService.AddToCart(dishId, quantity);
        return Ok();
    }

    [HttpPost("remove")]
    public IActionResult RemoveFromCart(int dishId)
    {
        _cartService.RemoveFromCart(dishId);
        return Ok();
    }
}
