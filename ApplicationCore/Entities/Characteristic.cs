using ApplicationCore.Extensions;
using ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Characteristic of a category. Like Storage for Smartphones or Weight for bycicles.
    /// </summary>
    public class Characteristic: Entity, IGenericMemberwiseClonable<Characteristic>
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; }
        public ICollection<CharacteristicValue> CharacteristicValues { get; set; } = new List<CharacteristicValue>();

        public Characteristic(): base()
        {
        }

        public bool Equals(Characteristic other) => base.Equals(other)
            && Title.EqualsCI(other.Title)
            && CategoryId == other.CategoryId;
        public override bool Equals(object obj) => Equals(obj as Characteristic);
        public override int GetHashCode() => base.GetHashCode();

        protected Characteristic(Characteristic characteristic): base(characteristic)
        {
            Title = characteristic.Title;
            CategoryId = characteristic.CategoryId;
        }

        public Characteristic ShallowClone()
        {
            return (Characteristic)MemberwiseClone();
        }
    }
}
