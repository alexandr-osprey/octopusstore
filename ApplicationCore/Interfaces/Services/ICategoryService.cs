using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    /// <summary>
    /// Maintains full lifecycle of Category entities
    /// </summary>
    public interface ICategoryService : IService<Category>
    {
        /// <summary>
        /// Id of primal Category
        /// </summary>
        Category RootCategory { get; }
        /// <summary>
        /// Enumerates Categories from specified category up to the root and subcategories. Used to flatten category hierarchy.
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task<IEnumerable<Category>> EnumerateHierarchyAsync(Specification<Category> spec);
        /// <summary>
        /// Enumerates Categories from specified item up to the root and subcategories. Used to flatten category hierarchy.
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task<IEnumerable<Category>> EnumerateHierarchyAsync(Specification<Item> spec);
        /// <summary>
        /// Enumerates Categories from specified category up to the root. Used to flatten category hierarchy.
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task<IEnumerable<Category>> EnumerateParentCategoriesAsync(Specification<Category> spec);
        /// <summary>
        /// Enumerates Categories from specified category down to the hierarchy. Used to flatten category hierarchy.
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task<IEnumerable<Category>> EnumerateSubcategoriesAsync(Specification<Category> spec);
        /// <summary>
        /// Enumerates Categories from specified item down to the hierarchy. Used to flatten category hierarchy.
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task<IEnumerable<Category>> EnumerateSubcategoriesAsync(Specification<Item> spec);
        /// <summary>
        /// Enumerates Categories of specified item(s) up to the root. Used to flatten category hierarchy.
        /// </summary>
        /// <param name="itemSpec"></param>
        /// <returns></returns>
        Task<IEnumerable<Category>> EnumerateParentCategoriesAsync(Specification<Item> itemSpec);
    }
}
