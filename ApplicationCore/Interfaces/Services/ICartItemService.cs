using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Maintains full lifecycle of cart of current user
    /// </summary>
    public interface ICartItemService : IService<CartItem>
    {
        /// <summary>
        /// Adds a number of item variants into user cart
        /// </summary>
        /// <param name="itemVariantId"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<CartItem> AddToCartAsync(int itemVariantId, int number);
        /// <summary>
        /// Enumerates all user cart items
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CartItem>> EnumerateCartItemsAsync();
        /// <summary>
        /// Removes a number of item variants into user cart
        /// </summary>
        /// <param name="itemVariantId"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        Task RemoveFromCartAsync(int itemVariantId, int number);
        /// <summary>
        /// Removes all items from user cart
        /// </summary>
        /// <returns></returns>
        Task ClearCartAsync();
    }
}
