using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class CategoryDetailSpecification : DetailSpecification<Category>
    {
        public CategoryDetailSpecification(int id)
            : base(id)
        {
            AddInclude("Subcategories.Subcategories.Subcategories.Subcategories");
            Description += " Includes Subcategories.Subcategories.Subcategories.Subcategories";
        }
    }
}
