using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Characteristic of a category. Like Storage for Smartphones or Weight for bycicles.
    /// </summary>
    public class Characteristic: Entity
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; }
        public ICollection<CharacteristicValue> CharacteristicValues { get; set; } = new List<CharacteristicValue>();
    }
}
