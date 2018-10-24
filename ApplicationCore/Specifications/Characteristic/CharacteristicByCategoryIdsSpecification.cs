using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Specifications
{
    public class CharacteristicByCategoryIdsSpecification : Specification<Characteristic>
    {
        public CharacteristicByCategoryIdsSpecification(IEnumerable<int> categoryIds)
            : base((c => categoryIds.Contains(c.CategoryId)))
        {
            Description = $"Characteristics with CategoryIds={string.Join(", ", categoryIds)}";
        }
    }
}
