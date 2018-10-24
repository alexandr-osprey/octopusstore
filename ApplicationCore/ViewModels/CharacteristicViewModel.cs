using ApplicationCore.Entities;
using ApplicationCore.ViewModels;

namespace ApplicationCore.ViewModels
{
    public class CharacteristicViewModel : EntityViewModel<Characteristic>
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }

        public CharacteristicViewModel()
            : base()
        {
        }
        public CharacteristicViewModel(Characteristic categoryProperty)
            : base(categoryProperty)
        {
            Title = categoryProperty.Title;
            CategoryId = categoryProperty.CategoryId;
        }

        public override Characteristic ToModel()
        {
            return new Characteristic()
            {
                Id = Id,
                Title = Title,
                CategoryId = CategoryId
            };
        }
        public override Characteristic UpdateModel(Characteristic modelToUpdate)
        {
            modelToUpdate.Title = Title;
            modelToUpdate.CategoryId = CategoryId;
            return modelToUpdate;
        }
    }
}
