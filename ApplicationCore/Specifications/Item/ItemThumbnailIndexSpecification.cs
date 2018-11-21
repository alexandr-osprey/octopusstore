using ApplicationCore.Entities;
using System.Collections.Generic;

namespace ApplicationCore.Specifications
{
    public class ItemThumbnailIndexSpecification: ItemIndexSpecification
    {
        public ItemThumbnailIndexSpecification(ItemIndexSpecification itemIndexSpecification): base(itemIndexSpecification)
        {
            SetProperties();
        }
        public ItemThumbnailIndexSpecification(int pageIndex, int pageSize, string title, IEnumerable<Category> categories, int? storeId, int? brandId, HashSet<int> characteristicValueIds)
           : base(pageIndex, pageSize, title, categories, storeId, brandId, characteristicValueIds)
        {
            SetProperties();
        }
        protected void SetProperties()
        {
            AddInclude(i => i.Images);
            AddInclude(i => i.ItemVariants);
            Description += " includes Images, ItemVariants";
        }
    }
}
