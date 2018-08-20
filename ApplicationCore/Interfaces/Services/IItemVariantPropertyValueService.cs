using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IItemVariantCharacteristicValueService : IService<ItemVariantCharacteristicValue>
    {
        Task<IEnumerable<ItemVariantCharacteristicValue>> ListByItemVariantAsync(ISpecification<ItemVariant> itemVariantSpec);
    }
}
