using ApplicationCore.Entities;
using ApplicationCore.ViewModels;

namespace OctopusStore.ViewModels
{
    public class BrandDetailViewModel : DetailViewModel<Brand>
    {
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
        public override void UpdateModel(Brand modelToUpdate)
        {
            modelToUpdate.Title = Title;
        }
    }
}
