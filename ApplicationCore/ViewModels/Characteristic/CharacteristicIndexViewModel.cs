using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public class CharacteristicIndexViewModel : EntityIndexViewModel<CharacteristicViewModel, Characteristic>
    {
        public CharacteristicIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<Characteristic> categoryProperties)
            : base(page, totalPages, totalCount, from categoryProperty in categoryProperties select new CharacteristicViewModel(categoryProperty))
        {
        }
    }
}
