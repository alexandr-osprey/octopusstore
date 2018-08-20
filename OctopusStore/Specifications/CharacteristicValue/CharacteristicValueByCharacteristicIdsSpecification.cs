using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace OctopusStore.Specifications
{
    public class CharacteristicValueByCharacteristicIdsSpecification : Specification<CharacteristicValue>
    {
        public CharacteristicValueByCharacteristicIdsSpecification(IEnumerable<int> characteristicIds)
            : base((c => characteristicIds.Contains(c.CharacteristicId)))
        {
            Description = $"CharacteristicValues with CharacteristicIds={string.Join(",", characteristicIds)}";
        }
    }
}
