using ApplicationCore.Entities;
using ApplicationCore.ViewModels;

namespace OctopusStore.ViewModels
{
    public class CharacteristicValueViewModel : ViewModel<CharacteristicValue>
    {
        public string Title { get; set; }
        public int CharacteristicId { get; set; }

        public CharacteristicValueViewModel()
            : base()
        { }
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
        public override void UpdateModel(CharacteristicValue modelToUpdate)
        {
            modelToUpdate.Title = Title;
            modelToUpdate.CharacteristicId = CharacteristicId;
        }
    }
}
