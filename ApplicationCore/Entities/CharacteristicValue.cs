using ApplicationCore.Extensions;
using ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Value of characteristic
    /// </summary>
    public class CharacteristicValue: Entity, ShallowClonable<CharacteristicValue>
    {
        public string Title { get; set; }
        public int CharacteristicId { get; set; }

        public virtual Characteristic Characteristic { get; set; }
        public virtual IEnumerable<ItemProperty> ItemProperties { get; set; } = new List<ItemProperty>();

        public CharacteristicValue(): base()
        {
        }

        public bool Equals(CharacteristicValue other) => base.Equals(other)
            && Title.EqualsCI(other.Title)
            && CharacteristicId == other.CharacteristicId;
        public override bool Equals(object obj) => Equals(obj as CharacteristicValue);
        public override int GetHashCode() => base.GetHashCode();

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
