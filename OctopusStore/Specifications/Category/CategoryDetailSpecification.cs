using ApplicationCore.Entities;

namespace OctopusStore.Specifications
{
    public class CategoryDetailSpecification : DetailSpecification<Category>
    {
        public CategoryDetailSpecification(int id)
            : base(id)
        {
            AddInclude("Subcategories.Subcategories.Subcategories.Subcategories");
        }
    }
}
