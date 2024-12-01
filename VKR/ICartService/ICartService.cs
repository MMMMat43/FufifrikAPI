namespace VKR.ICartService
{
    public interface ICartService
    {
        void AddToCart(int dishId, int quantity);
        void RemoveFromCart(int dishId);
        int GetCartCount();
    }

    public class CartService : ICartService
    {
        private readonly List<CartItem> _cartItems = new List<CartItem>();

        public void AddToCart(int dishId, int quantity)
        {
            var item = _cartItems.FirstOrDefault(x => x.DishId == dishId);
            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                _cartItems.Add(new CartItem { DishId = dishId, Quantity = quantity });
            }
        }

        public void RemoveFromCart(int dishId)
        {
            var item = _cartItems.FirstOrDefault(x => x.DishId == dishId);
            if (item != null)
            {
                _cartItems.Remove(item);
            }
        }

        public int GetCartCount()
        {
            return _cartItems.Sum(x => x.Quantity);
        }
    }

    public class CartItem
    {
        public int DishId { get; set; }
        public int Quantity { get; set; }
    }

}
