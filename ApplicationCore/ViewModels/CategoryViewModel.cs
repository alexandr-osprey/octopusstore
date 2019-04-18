using ApplicationCore.Entities;
using ApplicationCore.ViewModels;

namespace ApplicationCore.ViewModels
{
    public class CategoryViewModel : EntityViewModel<Category>
    {
        public string Title { get; set; }
        public int ParentCategoryId { get; set; }
        public string Description { get; set; }
        public bool IsRoot { get; set; }

        public CategoryViewModel(): base()
        {
        }

        public CategoryViewModel(Category category): base(category)
        {
            Title = category.Title;
            ParentCategoryId = category.ParentCategoryId;
            IsRoot = category.IsRoot;
        }

        public override Category ToModel()
        {
            return new Category()
            {
                Id = Id,
                Title = Title,
                ParentCategoryId = ParentCategoryId,
                IsRoot = IsRoot
            };
        }
        public override Category UpdateModel(Category modelToUpdate)
        {
            modelToUpdate.Title = Title;
            modelToUpdate.ParentCategoryId = ParentCategoryId;
            return modelToUpdate;
        }
    }
}
