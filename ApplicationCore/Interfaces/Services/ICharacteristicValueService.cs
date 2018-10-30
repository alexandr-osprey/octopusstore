using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    /// <summary>
    /// Maintains full lifecycle of CharacteristicValue entities
    /// </summary>
    public interface ICharacteristicValueService: IService<CharacteristicValue>
    {
        /// <summary>
        /// Enumerates CharactersticValues of Item described by specification
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task<IEnumerable<CharacteristicValue>> EnumerateByItemAsync(Specification<Item> spec);
        /// <summary>
        /// Enumerates CharactersticValues of Categories described by specification
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task<IEnumerable<CharacteristicValue>> EnumerateByCategoryAsync(Specification<Category> spec);
    }
}
