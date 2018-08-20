using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ICategoryService : IService<Category>
    {
        int RootCategoryId { get; set; }
        Task<IEnumerable<Category>> ListHierarchyAsync(ISpecification<Category> spec);
        Task<IEnumerable<Category>> ListSubcategoriesAsync(ISpecification<Category> spec);
        Task<IEnumerable<Category>> ListByItemAsync(ISpecification<Item> itemSpec);
    }
}
