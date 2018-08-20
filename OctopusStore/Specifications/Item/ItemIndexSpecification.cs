using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace OctopusStore.Specifications
{
    public class ItemIndexSpecification : BaseSpecification<Item>
    {
        public ItemIndexSpecification(int pageIndex, int pageSize, string title, IEnumerable<Category> categories, int? storeId, int? brandId)
            : base(i => (!HasValue(title) || i.Title.Contains(title, System.StringComparison.OrdinalIgnoreCase)) &&
                        (!(categories != null && categories.Count() > 0) || categories.Where(c => c.Id == i.CategoryId).Any()) &&
                        (!storeId.HasValue || i.StoreId == storeId) &&
                        (!brandId.HasValue || i.BrandId == brandId))
        {
            Take = pageSize;
            Skip = Take * (pageIndex - 1);

            Description = $"{nameof(pageIndex)}: {pageIndex}, {nameof(pageSize)}: {pageSize}";
            if (HasValue(title))     Description +=  $", { nameof(title)}: {title}";
            if (categories != null) Description +=  $", {nameof(categories)}: {string.Join(", ", categories)}";
            if (storeId.HasValue)   Description +=  $", {nameof(storeId)}: {storeId}";
            if (brandId.HasValue)   Description +=  $", {nameof(brandId)}: {brandId}";
        }
    }
}
