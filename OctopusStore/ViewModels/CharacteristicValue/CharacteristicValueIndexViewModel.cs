using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace OctopusStore.ViewModels
{
    public class CharacteristicValueIndexViewModel : IndexViewModel<CharacteristicValueViewModel, CharacteristicValue>
    {
        public CharacteristicValueIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<CharacteristicValue> categoryPropertyValues)
            : base(page, totalPages, totalCount, from categoryPropertyValue in categoryPropertyValues select new CharacteristicValueViewModel(categoryPropertyValue))
        {  }
    }
}
