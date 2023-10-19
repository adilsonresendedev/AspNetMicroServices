namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {

        }

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; set; } = default!;
        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();

        public decimal TotalPrice { get { return ShoppingCartItems.Sum(x => x.Price); }}
    }
}
