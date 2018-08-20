using ApplicationCore.Entities;
using ApplicationCore.ViewModels;

namespace OctopusStore.ViewModels
{
    public class BrandViewModel : ViewModel<Brand>
    {
        public string Title { get; set; }

        public BrandViewModel(Brand brand)
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
