using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public class CharacteristicValueDetailViewModel : EntityDetailViewModel<CharacteristicValue>
    {
        public string Title { get; set; }
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
        public override CharacteristicValue UpdateModel(CharacteristicValue modelToUpdate)
        {
            modelToUpdate.Title = Title;
            modelToUpdate.CharacteristicId = CharacteristicId;
            return modelToUpdate;
        }
    }
}
