using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Maintains full lifecycle of Item entities
    /// </summary>
    public interface IItemService : IService<Item>
    {
        /// <summary>
        /// Creates ItemIndexSpecification based on provided parameters.
        /// Retrieves subactetories of category if provided.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="title"></param>
        /// <param name="categoryId"></param>
        /// <param name="storeId"></param>
        /// <param name="brandId"></param>
        /// <returns></returns>
        Task<ItemIndexSpecification> GetIndexSpecificationByParameters(int page, int pageSize, string title, int? categoryId, int? storeId, int? brandId);
    }
}
