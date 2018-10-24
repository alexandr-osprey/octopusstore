using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Maintains full lifecycle of Characteristic entities
    /// </summary>
    public interface ICharacteristicService : IService<Characteristic>
    {
        /// <summary>
        /// Enumerates Characterstics of Items described by specification
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task<IEnumerable<Characteristic>> EnumerateAsync(Specification<Item> spec);
        /// <summary>
        /// Enumerates Characterstics of Categories described by specification
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task<IEnumerable<Characteristic>> EnumerateByCategoryAsync(Specification<Category> spec);
    }
}
