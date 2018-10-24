using ApplicationCore.Entities;
using ApplicationCore.ViewModels;

namespace ApplicationCore.ViewModels
{
    public class CharacteristicValueViewModel : EntityViewModel<CharacteristicValue>
    {
        public string Title { get; set; }
        public int CharacteristicId { get; set; }

        public CharacteristicValueViewModel()
            : base()
        {
        }
        public CharacteristicValueViewModel(CharacteristicValue categoryPropertyValue)
            : base(categoryPropertyValue)
        {
            Title = categoryPropertyValue.Title;
            CharacteristicId = categoryPropertyValue.CharacteristicId;
        }

        public override CharacteristicValue ToModel()
        {
            return new CharacteristicValue()
            {
                Id = Id,
                Title = Title,
                CharacteristicId = CharacteristicId
            };
        }
        public override CharacteristicValue UpdateModel(CharacteristicValue modelToUpdate)
        {
            modelToUpdate.Title = Title;
            modelToUpdate.CharacteristicId = CharacteristicId;
            return modelToUpdate;
        }
    }
}
