using ApplicationCore.Entities;
using ApplicationCore.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Specifications
{
    public class ItemIndexSpecification: EntitySpecification<Item>
    {
        public ItemIndexSpecification(ItemIndexSpecification itemIndexSpecification): base(itemIndexSpecification)
        {
        }

        public ItemIndexSpecification(int pageIndex, int pageSize, string title, IEnumerable<Category> categories, int? storeId, int? brandId)
           : base(i => (!HasValue(title) || i.Title.Contains(title, System.StringComparison.OrdinalIgnoreCase)) &&
                        (!(categories != null && categories.Count() > 0) || categories.Where(c => c.Id == i.CategoryId).Any()) &&
                        (!storeId.HasValue || i.StoreId == storeId) &&
                        (!brandId.HasValue || i.BrandId == brandId))
        {
            Take = pageSize;
            Skip = Take * (pageIndex - 1);

            Description = $"{nameof(pageIndex)}: {pageIndex}, {nameof(pageSize)}: {pageSize}" +
                $", {nameof(title)}: {title ?? "null"}" +
                $", {nameof(categories)}: {string.Join(", ", categories)}" +
                $", {nameof(storeId)} (0 - not provided): {storeId ?? 0}" +
                $", {nameof(brandId)} (0 - not provided): {brandId ?? 0}";
        }
    }
}
