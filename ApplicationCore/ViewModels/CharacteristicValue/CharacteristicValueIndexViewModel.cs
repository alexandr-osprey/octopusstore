using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public class CharacteristicValueIndexViewModel : EntityIndexViewModel<CharacteristicValueViewModel, CharacteristicValue>
    {
        public CharacteristicValueIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<CharacteristicValue> categoryPropertyValues)
            : base(page, totalPages, totalCount, from categoryPropertyValue in categoryPropertyValues select new CharacteristicValueViewModel(categoryPropertyValue))
        {
        }
    }
}
