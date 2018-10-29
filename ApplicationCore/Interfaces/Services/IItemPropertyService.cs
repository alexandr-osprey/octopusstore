using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Maintains full lifecycle of ItemProperty entities
    /// </summary>
    public interface IItemPropertyService: IService<ItemProperty>
    {
        /// <summary>
        /// Enumerates ItemProperty based on specified ItemVariant
        /// </summary>
        /// <param name="itemVariantSpec"></param>
        /// <returns></returns>
        Task<IEnumerable<ItemProperty>> EnumerateByItemVariantAsync(Specification<ItemVariant> itemVariantSpec);
    }
}
