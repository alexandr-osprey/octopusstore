using ApplicationCore.Entities;
using ApplicationCore.ViewModels;

namespace OctopusStore.ViewModels
{
    public class CharacteristicValueDetailViewModel : DetailViewModel<CharacteristicValue>
    {
        public int CharacteristicId { get; set; }

        public CharacteristicValueDetailViewModel(CharacteristicValue categoryPropertyValue)
            : base(categoryPropertyValue)
        {
            Title = Title;
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
