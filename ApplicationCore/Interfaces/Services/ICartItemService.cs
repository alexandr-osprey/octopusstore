using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
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
        Task<CartItem> AddToCartAsync(string ownerId, int itemVariantId, int number);
        /// <summary>
        /// Removes a number of item variants into user cart
        /// </summary>
        /// <param name="itemVariantId"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        Task RemoveFromCartAsync(string ownerId, int itemVariantId, int number);
    }
}
