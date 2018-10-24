using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Maintains full lifecycle of ItemVariantCharacteristic entities
    /// </summary>
    public interface IItemVariantCharacteristicValueService : IService<ItemVariantCharacteristicValue>
    {
        /// <summary>
        /// Enumerates ItemVariantCharacteristicValue based on specified ItemVariant
        /// </summary>
        /// <param name="itemVariantSpec"></param>
        /// <returns></returns>
        Task<IEnumerable<ItemVariantCharacteristicValue>> EnumerateByItemVariantAsync(Specification<ItemVariant> itemVariantSpec);
    }
}
