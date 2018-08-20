using ApplicationCore.Entities;
using System.Collections.Generic;

namespace OctopusStore.Specifications
{
    public class ItemThumbnailIndexSpecification : ItemIndexSpecification
    {
        public ItemThumbnailIndexSpecification(int pageIndex, int pageSize, string title, IEnumerable<Category> categories, int? storeId, int? brandId)
            : base(pageIndex, pageSize, title, categories, storeId, brandId)
        {
            AddInclude(i => i.Images);
            AddInclude(i => i.ItemVariants);
            Description += " includes Images, ItemVariants";
        }
    }
}
