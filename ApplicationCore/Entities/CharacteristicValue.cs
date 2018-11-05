using ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Value of characteristic
    /// </summary>
    public class CharacteristicValue: Entity, IGenericMemberwiseClonable<CharacteristicValue>
    {
        public string Title { get; set; }
        public int CharacteristicId { get; set; }

        public Characteristic Characteristic { get; set; }
        public IEnumerable<ItemProperty> ItemProperties { get; set; } = new List<ItemProperty>();

        public CharacteristicValue(): base()
        {
        }

        protected CharacteristicValue(CharacteristicValue characteristicValue): base(characteristicValue)
        {
            Title = characteristicValue.Title;
            CharacteristicId = characteristicValue.CharacteristicId;
        }

        public CharacteristicValue ShallowClone()
        {
            return (CharacteristicValue)MemberwiseClone();
        }
    }
}
