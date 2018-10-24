using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public class ItemVariantCharacteristicValueIndexViewModel : EntityIndexViewModel<ItemVariantCharacteristicValueViewModel, ItemVariantCharacteristicValue>
    {
        public ItemVariantCharacteristicValueIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<ItemVariantCharacteristicValue> itemVariantCharacteristicValues)
            : base(page, totalPages, totalCount, from itemVariant in itemVariantCharacteristicValues select new ItemVariantCharacteristicValueViewModel(itemVariant))
        {
        }
    }
}
