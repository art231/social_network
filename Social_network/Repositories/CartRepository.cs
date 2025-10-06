using Social_network.Models.Entities;

namespace Social_network.Repositories
{
    public class CartRepository
    {
        // TODO: Implement cart repository methods
        public Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return Task.FromResult<Cart?>(null);
        }

        public Task<bool> AddItemToCartAsync(int cartId, int productId, int quantity, decimal price)
        {
            return Task.FromResult(true);
        }

        public Task<bool> UpdateCartItemAsync(int itemId, int quantity)
        {
            return Task.FromResult(true);
        }

        public Task<bool> RemoveCartItemAsync(int itemId)
        {
            return Task.FromResult(true);
        }

        public Task<bool> ClearCartAsync(int cartId)
        {
            return Task.FromResult(true);
        }
    }
}
