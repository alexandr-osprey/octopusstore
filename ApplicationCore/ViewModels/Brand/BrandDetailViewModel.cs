using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public class BrandDetailViewModel : EntityDetailViewModel<Brand>
    {
        public string Title { get; set; }

        public BrandDetailViewModel(Brand brand)
            : base(brand)
        {
            Title = brand.Title;
        }

        public override Brand ToModel()
        {
            return new Brand()
            {
                Id = Id,
                Title = Title
            };
        }
        public override Brand UpdateModel(Brand modelToUpdate)
        {
            modelToUpdate.Title = Title;
            return modelToUpdate;
        }
    }
}
