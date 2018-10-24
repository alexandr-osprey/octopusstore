using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public class CategoryDetailViewModel  : EntityDetailViewModel<Category>
    {
        public string Title { get; set; }
        public int ParentCategoryId { get; set; }
        public string Description { get; set; }

        public CategoryDetailViewModel(Category category)
            : base(category)
        {
            Title = category.Title;
            ParentCategoryId = category.ParentCategoryId;
            Description = category.Description;
        }

        public override Category ToModel()
        {
            return new Category()
            {
                Id = Id,
                Title = Title,
                ParentCategoryId = ParentCategoryId,
                Description = Description
            };
        }
        public override Category UpdateModel(Category modelToUpdate)
        {
            modelToUpdate.Title = Title;
            modelToUpdate.ParentCategoryId = ParentCategoryId;
            modelToUpdate.Description = Description;
            return modelToUpdate;
        }
    }
}
