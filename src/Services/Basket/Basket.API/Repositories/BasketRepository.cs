using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _distributedCache;

        public BasketRepository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task DeleteBasket(string userName)
        {
            await _distributedCache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            ShoppingCart shoppingCart = new ShoppingCart();
            string shoppingCartString = await _distributedCache.GetStringAsync(userName);
            if(string.IsNullOrEmpty(shoppingCartString))
            {
                return shoppingCart;
            }

            shoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(shoppingCartString);
            return shoppingCart;
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
        {
            await _distributedCache.SetStringAsync(shoppingCart.UserName, JsonConvert.SerializeObject(shoppingCart));

            return await GetBasket(shoppingCart.UserName);
        }
    }
}
